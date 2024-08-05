using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

public class EmailSenderByYandexService : IEmailSender
{
    private readonly EmailOptions _emailOptions;
    private readonly ILogger<EmailSenderByYandexService> _logger;
    private readonly BaseEmailSenderService _baseSender;

    public EmailSenderByYandexService(IOptions<EmailOptions> options,
        ILogger<EmailSenderByYandexService> logger, BaseEmailSenderService baseSender)
    {
        _emailOptions = options.Value;
        _logger = logger;
        _baseSender = baseSender;
    }

    public async Task SendAsync(string toAddress, string title, string htmlBody)
    {
        await _baseSender.Send(_emailOptions.Address, _emailOptions.Password, toAddress,
            "smtp.yandex.ru", 587, title, htmlBody);
    }
}