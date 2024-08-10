using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[ApiController, Route("/api")]
public class LoginController : ControllerBase
{
    private readonly IHasher _hasher;
    private readonly IHashVerify _hashVerify;
    private readonly ILogger<LoginController> _logger;
    private readonly IEmailVerify _emailVerify;
    private readonly JwtService _jwtService;
    private readonly VerfiyCodeOptions _verifierCodeOptions;
    private readonly UserService _userService;
    private readonly RefreshTokenService _refreshService;
    public LoginController(IHasher hasher, ILogger<LoginController> logger,
        IHashVerify hashVerify, IEmailVerify emailVerify,
        JwtService jwtService, IOptions<VerfiyCodeOptions> options,
        UserService userService, RefreshTokenService refreshTokenService)
    {
        _hasher = hasher;
        _logger = logger;
        _hashVerify = hashVerify;

        _emailVerify = emailVerify;
        _jwtService = jwtService;

        _verifierCodeOptions = options.Value;
        _userService = userService;
        _refreshService = refreshTokenService;
    }

    [HttpPost, Route("login"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> Login([FromForm] UserLoginDto dto)
    {
        var findUsers = await _userService.GetUsersByEmail(dto.Email);

        if (findUsers is not null && findUsers.Count() > 0)
        {
            var findUser = findUsers
                .FirstOrDefault(x => _hashVerify.Verify(dto.Password, x.PasswordHash));

            if (findUser is null)
                return BadRequest("Invalid password");

            try
            {
                await _emailVerify.CodeSend(findUser.Id, findUser.Email);
            }
            catch (SmtpException e)
            {
                return StatusCode(((int)e.StatusCode), e.Message);
            }

            return Ok(new
            {
                UserId = findUser.Id.ToString(),
                CodeDiedAfterSeconds = _verifierCodeOptions.DiedAfterSeconds.ToString(),
                CodeLength = _verifierCodeOptions.Length.ToString()
            });
        }
        return NotFound("The user was not found with such a email");
    }

    [HttpPost, Route("accountcreate"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> AccountCreate([FromForm] UserRegistrationDto dto)
    {
        var existingUser = (await _userService.GetUsersByEmail(dto.Email))
            .FirstOrDefault(x => x.EmailVerify
            || _hashVerify.Verify(dto.Password, x.PasswordHash));
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
                await _userService.Add(newUser);//User create, вобще в идиале нужна бд, но мне лишь нужно потыкать реакт который работает с asp.net
                _logger.LogDebug("Created user: {0}", newUser.Name);

                try
                {
                    await _emailVerify.CodeSend(newUser.Id, newUser.Email);
                }
                catch (SmtpException e)
                {
                    return StatusCode(((int)e.StatusCode), e.Message);
                }

                return Ok(new
                {
                    UserId = newUser.Id.ToString(),
                    CodeDiedAfterSeconds = _verifierCodeOptions.DiedAfterSeconds.ToString(),
                    CodeLength = _verifierCodeOptions.Length.ToString()
                });
            }
            return BadRequest();
        }
        return Conflict("A user with such an email already exists");
    }


    [HttpPut, Route("coderesend/{userId}"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> CodeResend(Guid userId)
    {
        var user = await _userService.GetUser(userId);

        if (user is null)
            return NotFound("User not found");

        try
        {
            await _emailVerify.Resend(userId, user.Email);
        }
        catch (SmtpException e)
        {
            return StatusCode(((int)e.StatusCode), e.Message);
        }

        return Ok(new
        {
            CodeDiedAfterSeconds = _verifierCodeOptions.DiedAfterSeconds.ToString(),
            CodeLength = _verifierCodeOptions.Length.ToString()
        });
    }
    [HttpGet, Route("userinfo"), Authorize, ValidationFilter]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = User.Claims.GetIdValue();

        if (userId == null)
            return BadRequest();

        var user = await _userService.GetUser(new Guid(userId));

        return Ok(user != null
            ? new UserDto { Id = user.Id.ToString(), Email = user.Email, Name = user.Name }
            : null);
    }

    [HttpPost, Route("emailverify"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> EmailVerify([FromBody] EmailVerifyQuery query)//Я даун, дал FromBody БЛЯТЬ ГЕТ ЗАПРОСУ!!! 
    //Забыл что get не имеет тело в основном, потому asp.net прихуел от того что клиент посылает гет с телом
    {
        var findUser = await _userService.GetUser(query.UserId);

        if (findUser is null)
            return NotFound("User not found");

        var verifyRes = await _emailVerify.CodeVerify(query.UserId, query.Code);

        if (verifyRes)
        {
            await _userService.EmailVerUpdate(findUser.Id, true);
            var tokens = await TokensCreate(findUser);

            var res = (await _refreshService.GetUser(tokens.RefreshToken)) is null
                ? await _refreshService.Add(findUser.Id, tokens.RefreshToken)
                : await _refreshService.UpdateToken(findUser.Id, tokens.RefreshToken);

            return Ok(tokens);
        }
        return BadRequest();
    }

    //Authorize буду юзать лишь для Акцес, тк он и будет основным токеном, рефреш нужен лишь для создания новой пары акцес и рефреш
    //Только Анон  тк у меня проверка именно акцес токена.
    [HttpPut, Route("tokensupdate"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> TokensUpdate([FromBody] TokensUpdateQuery query)//Объявив параметр refresh с [FromBody] вы говорите
    // ASP.NET Core использовать форматировщик ввода для привязки предоставленного JSON (или XML) к модели
    //БЛЯТЬ, короче, там нужны кастомные типы, то есть странно, но с refresh-ом в json этот не удет соединяться, тк не в кастом типе
    {
        var user = await _jwtService.GetUserFromRefreshToken(query.RefreshToken);

        if (user is not null)// && RefreshTokenService.RefreshTokenVerify(refresh)
        {
            var tokens = await TokensCreate(user);

            await _refreshService.UpdateToken(user.Id, tokens.RefreshToken);
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