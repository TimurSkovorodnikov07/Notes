using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:4505");

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var jwtOptions =
    builder.Configuration.GetRequiredSection("UserSecrets:Jwt").Get<JwtOptions>();

builder.Services.AddHealthChecks()
    .AddCheck<RedisConnectionHealthCheck>(nameof(RedisConnectionHealthCheck));

builder.Services.AddStackExchangeRedisCache((opts) =>
{
    string connectionStr = builder.Configuration["UserSecrets:RedisConnectionStr"];

    if (string.IsNullOrEmpty(connectionStr))
        throw new ArgumentNullException("Redis connection string is empty");

    opts.Configuration = connectionStr;
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors((opts) =>
{
    opts.AddPolicy("defaultCorsPolicy", (builder) =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer((opts) =>
{
    opts.RequireHttpsMetadata = false/*TODO true*/;

    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAlgorithms = new List<string> { jwtOptions.AlgorithmForAccessToken }, //Валидные алгоритмы(если токен не юзает именно эти он не вал)
        ValidateIssuerSigningKey = true,//валидация ключа безопасности
        IssuerSigningKey = jwtOptions.GetAccessSymmetricSecurityKey(),//установка ключа безопасности

        ValidateLifetime = true,//будет ли валидироваться время существования
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => //Валидатор времени
        {
            if (expires != null)
                return expires.Value > DateTime.UtcNow;

            return false;
        },
        //Тут пишуться валидные значения, чтобы потом аут через токен(проверка на валидность)
    };
});
builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    o.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Description = "Basic auth added to authorization header",
        Name = "Authorization",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http
    });

    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" },

                Name = "Authorization",
                Type = SecuritySchemeType.Http
            },
            new List<string>()
        }
    });
});

//Add IOptions
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetRequiredSection("UserSecrets:Jwt"));
builder.Services.Configure<VerfiyCodeOptions>(
    builder.Configuration.GetRequiredSection("VerifyCode"));
builder.Services.Configure<HealthOptions>(
    builder.Configuration.GetRequiredSection("Health"));
builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetRequiredSection("UserSecrets:Email"));

//Add services
builder.Services.AddSingleton<ConnectionFactory>(new ConnectionFactory(
    builder.Configuration
    .GetRequiredSection("UserSecrets:PostgresConnectionStr")
    .Get<string>()));
builder.Services.AddSingleton<QueryCreaterService>();
builder.Services.AddSingleton<ValidationFilter>();

builder.Services.AddSingleton<ITokenNameInCookies>(jwtOptions);
builder.Services.AddSingleton<BaseEmailSenderService>();
builder.Services.AddSingleton<IHasher, HashingManagerService>();
builder.Services.AddSingleton<IHashVerify, HashingManagerService>();


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<ICodeCreator, CodeService>();
builder.Services.AddScoped<IEmailSender, EmailSenderByYandexService>();
builder.Services.AddScoped<IEmailVerify, EmailVerifyService>();

var app = builder.Build();

app.UseFavicon();//Этот мидлвеер нужен чтобы можно было спокойно открывать сайт в браузере, у меня свагер перестал показывать картинку(хз почему), потому браузер(Brave) пишет 400.
// ольшая часть браузеров требуют картинки для сайтов, ну а какаая картиинка может быть у апи? Ну и вот зачем этот мидлвеер
// Я даун, взял AllowedHosts(appSettings.Development.json) изменил на "localhost:4505" когда там просили хосты(домены) без портов
//Ихменил на localhost, теперь этот мидрвеер не нужен, но пусть будет, на память о том как я проебал часов 4-6 на решение тупой проблемы.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseGlobalExceptionHandler();//Сверху потому что ковеер если полуачет exception идет сниху вверх
    app.UseHttpsRedirection();
}
app.UseRouting();
app.UseCors("defaultCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();
app.Run();