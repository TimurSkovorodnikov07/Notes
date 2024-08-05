using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

public class EmailVerifyService : IEmailVerify
{
    private readonly VerfiyCodeOptions _options;
    private readonly IDistributedCache _distributedCache;
    private readonly IEmailSender _emailSend;
    private readonly ICodeCreator _codeCreator;
    private readonly IHasher _hasher;
    private readonly IHashVerify _hashVerify;
    private readonly ILogger<EmailVerifyService> _logger;
    public EmailVerifyService(IOptions<VerfiyCodeOptions> options, ILogger<EmailVerifyService> logger,
     IDistributedCache distributedCache, IEmailSender emailSend, ICodeCreator codeCreator,
     IHasher hasher, IHashVerify hashVerify)
    {
        _options = options.Value;
        _logger = logger;
        _distributedCache = distributedCache;
        _emailSend = emailSend;
        _codeCreator = codeCreator;
        _hasher = hasher;
        _hashVerify = hashVerify;
    }

    public async Task CodeSend(Guid userId, string email)
    {
        var code = _codeCreator.Create(_options.Length);
        var hashedCode = _hasher.Hashing(code);

        await _distributedCache.SetStringAsync(userId.ToString(), hashedCode,
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.DiedAfterSeconds)
        });

        string title = _options.EmailMessageTitle;
        string htmlBody = _options.EmailMessageHtmlBody.Replace("{CODE}", code.ToString());

        _logger.LogInformation("The message was sent to email: " + email);
        await _emailSend.SendAsync(email, title, htmlBody);
    }

    public async Task Resend(Guid userId, string email)
    {
        await _distributedCache.RemoveAsync(userId.ToString());
        await CodeSend(userId, email);
    }

    public async Task<bool> CodeVerify(Guid userId, string code)
    {
        var codeInCache = await _distributedCache.GetStringAsync(userId.ToString());

        return code is not null && codeInCache is not null
            && _hashVerify.Verify(code, codeInCache)
            ? true : false;
    }
}