public interface IEmailSender
{
    public Task SendAsync(string toAddress, string title, string htmlBody);
}