public class FaviconMiddleware

{
    public const string PATHTOFAVICON = "/home/timur/Desktop/FullStackProj1/serverApp/wwwroot/favicon.ico";
    private readonly RequestDelegate _next;
    public FaviconMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Value == "/favicon.ico")
        {
            if (!File.Exists(PATHTOFAVICON))
            {
                throw new FileNotFoundException($"No icons were found on path {PATHTOFAVICON}" +
                    "(It is needed, without it, some browsers do not open the site)");
            }

            await context.Response.SendFileAsync(PATHTOFAVICON);
            return;
        }
        await _next(context);
    }
}