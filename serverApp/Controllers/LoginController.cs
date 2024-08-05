using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[ApiController, Route("/api"), ValidationFilter]
public class LoginController : ControllerBase
{
    private readonly IHasher _hasher;
    private readonly IHashVerify _hashVerify;
    private readonly ILogger<LoginController> _logger;
    private readonly IEmailVerify _emailVerify;
    private readonly JwtService _jwtService;
    private readonly VerfiyCodeOptions _verifierCodeOptions;
    public LoginController(IHasher hasher, ILogger<LoginController> logger,
        IHashVerify hashVerify, IEmailVerify emailVerify,
        JwtService jwtService, IOptions<VerfiyCodeOptions> options)
    {
        _hasher = hasher;
        _logger = logger;
        _hashVerify = hashVerify;

        _emailVerify = emailVerify;
        _jwtService = jwtService;

        _verifierCodeOptions = options.Value;
    }

    [HttpPost, Route("login"), AnonymousOnly]
    public async Task<IActionResult> Login([Required, FromForm] UserLoginDto dto)
    {
        var findUsers = UserService.Users.Where(x => x.Email == dto.Email);

        if (findUsers is not null || findUsers.Count() > 0)
        {
            var findUser = findUsers
                .FirstOrDefault(x => _hashVerify.Verify(dto.Password, x.PasswordHash));

            if (findUser is null)
                return BadRequest("Invalid password");

            await _emailVerify.CodeSend(findUser.Id, findUser.Email);
            return Ok(new
            {
                UserId = findUser.Id,
                DiedAfterSeconds = _verifierCodeOptions.DiedAfterSeconds.ToString(),
                CodeLength = _verifierCodeOptions.Length.ToString()
            });
        }
        return NotFound("The user was not found with such a email");
    }

    [HttpPost, Route("accountcreate"), AnonymousOnly]
    public async Task<IActionResult> AccountCreate([Required, FromForm] UserRegistrationDto dto)
    {
        var existingUser = UserService.Users
            .FirstOrDefault(x => x.Email == dto.Email &&
            (x.EmailVerify || _hashVerify.Verify(dto.Password, x.PasswordHash)));
        //Зачем я беру юзера у которого есть подверждение или такой же пороль?
        //Чтобы хитрый школотрон не смог поломать мне все нахуй
        //Дело в том что AccountCreate ДО мог создать аккаунты с одинаковыми почтами и поролями, тк они было не подтверждены, потому добавил _hashVerify.Verify

        //Ну типо даун берет создает акк(у нее id = 1), нечаяно закрыл вкладку перед проверкой почты, после думает что акк не создан(тупой юзер же)
        //Создает еще раз с такой же почтой и поролем(уже с id = 2), подверждает почту а после что то делает на акк(с id = 2).
        //Заходит спустя полгода но акк с id = 1 и ахуевате хули у него он пустой, даун пишет что ему все снесли хотя его все богаство осталось в акке с id = 2

        if (existingUser == null)
        {
            var passwordHash = _hasher.Hashing(dto.Password);
            var newUser = UserEntity.Create(dto, passwordHash);

            if (newUser != null)
            {
                UserService.Users.Add(newUser);//User create, вобще в идиале нужна бд, но мне лишь нужно потыкать реакт который работает с asp.net
                _logger.LogDebug("Created user: {0}", newUser.Name);

                await _emailVerify.CodeSend(newUser.Id, newUser.Email);
                return Ok(new
                {
                    UserId = newUser.Id,
                    DiedAfterSeconds = _verifierCodeOptions.DiedAfterSeconds.ToString(),
                    CodeLength = _verifierCodeOptions.Length.ToString()
                });
            }
            return BadRequest();
        }
        return Conflict("A user with such an email already exists");
    }


    [HttpPut, Route("coderesend/{userId}"), AnonymousOnly]
    public async Task<IActionResult> CodeResend(string userId)
    {
        var id = new Guid(userId);
        var user = UserService.Users.FirstOrDefault(u => u.Id == id);

        if (user is null)
            return NotFound("User not found");

        await _emailVerify.Resend(id, user.Email);
        return Ok(new
        {
            DiedAfterSeconds = _verifierCodeOptions.DiedAfterSeconds.ToString(),
            CodeLength = _verifierCodeOptions.Length.ToString()
        });
    }
    [HttpGet, Route("userinfo"), Authorize]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = User.Claims.GetIdValue();

        if (userId == null)
            return BadRequest();

        var user = UserService.Users.FirstOrDefault((u) => u.Id.ToString() == userId);

        return Ok(user != null
            ? new UserDto { Id = user.Id.ToString(), Email = user.Email, Name = user.Name }
            : null);
    }

    [HttpGet, Route("emailverify/{userId}/{code}"), AnonymousOnly]
    public async Task<IActionResult> EmailVerify(string userId, string code)
    {
        var id = new Guid(userId);
        var findUser = UserService.Users.FirstOrDefault(u => u.Id == id);

        if (findUser is null)
            return NotFound("User not found");

        var verifyRes = await _emailVerify.CodeVerify(id, code);

        if (verifyRes)
        {
            findUser.EmailVerify = true;
            var tokens = await TokensCreate(findUser);

            await RefreshTokenService.AddRefreshToken(findUser.Id, tokens.RefreshToken);
            return Ok(tokens);
        }
        return BadRequest();
    }

    //Authorize буду юзать лишь для Акцес, тк он и будет основным токеном, рефреш нужен лишь для создания новой пары акцес и рефреш
    //Только Анон  тк у меня проверка именно акцес токена.
    [HttpGet, Route("tokensupdate"), AnonymousOnly]
    public async Task<IActionResult> TokensUpdate([FromBody, Required] string refresh)
    {
        var user = _jwtService.GetUserFromRefreshToken(refresh);

        if (user is not null && RefreshTokenService.RefreshTokenVerify(refresh))
        {
            var tokens = await TokensCreate(user);

            await RefreshTokenService.AddRefreshToken(user.Id, tokens.RefreshToken);
            return Ok(tokens);
        }

        return NotFound("Refresh token not found");
    }
    [NonAction]
    private async Task<Tokens> TokensCreate(UserEntity user)
    {
        var accessToken = _jwtService.AccessTokenCreate(user);
        var refreshToken = _jwtService.RefreshTokenCreate(user);

        return new(accessToken, refreshToken);
    }

    // [HttpGet, Route("accessdenied")]
    // public void AccessDenied()
    // {
    //     // return Forbid(); почему такой код нельзя писать? Asp.net(с Auth через Cookie, вроде)на возникновения ForbidResult делает редирект в этот action
    //     // ну и это как безконечный цикл будет

    //     HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
    //     //Ахуеть конечно, я вобще не сразу прикол это выкупил, лишь через полчаса зашел наконец в консоль и увидел лог с редирект на адрес accessdenied
    //     //Правда там вот returnUrl был очень длинным, вот и понял что дело в безконечных действиях
    //Щас жтот метод мне не нужен, но пусть будет, такой прикол забывать не стоит.
    // }
}