using MailKit.Net.Smtp;
using Microsoft.IdentityModel.Tokens;
using MimeKit;

public class BaseEmailSenderService
{
    private readonly ILogger<EmailSenderByYandexService> _logger;
    public BaseEmailSenderService(ILogger<EmailSenderByYandexService> logger)
    {
        _logger = logger;
    }
    public async Task Send(string fromAddress, string emailPas, string toAddress, string host, int port, string title, string htmlBody)
    {
        using var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Admin", fromAddress));
        message.To.Add(new MailboxAddress("", toAddress));
        message.Subject = title;
        message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlBody };

        //Юзал гугл, получал ошибку аут, ну ок, залезал в аккаунт, хотел для исправления ошибки добавить 2 факторную аут, а у меня номер +7 нахуй, вот и ебанные жиды из гугла посчитали что я росиянин, убийца, насильник и вор туалетов,
        //потому была хуйня что то вроде "Что то произошло не так, повторите попытку". Нет, ну жто пиздец, эту хуйню не обойти, долбоебам не скажешь что я не из РФ, пиздец.
        //https://www.youtube.com/watch?v=Kbndp8_cDYc КАКОЙ АХУЕННЫЙ ВИДОС И АВТОР!!!! Все! Заработало наконец!!!!
        //SMTP — это широко используемый сетевой протокол, предназначенный для передачи электронной почты в сетях TCP/IP. 
        //https://qna.habr.com/q/1221954 там про то как юзать отправку через яндекс, все тоже самое лишь дизайн сайта новый.

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(fromAddress, emailPas);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}