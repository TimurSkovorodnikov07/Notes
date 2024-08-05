public interface IEmailVerify
{
    public Task CodeSend(Guid userId, string email);
    public Task<bool> CodeVerify(Guid userId, string code);
    public Task Resend(Guid userId, string email);
}
//Не вижу смысла делить на две интерфейса, да да, солид спит, мен пох, все ровно коды отправляет приложение и проверяет тоже лишь одном контроллере, в др они не нужны будут