namespace Middelware;
//שאלה למורה: איך אפשר לדעת איזה ניים-ספיס לעשות?
public class Loger
{
    private RequestDelegate next;
    private readonly ILogger<Loger> logger;
    public Loger(RequestDelegate next,ILogger<Loger> logger)
    {
        this.next = next;
        this.logger=logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        logger.LogInformation($"requst:{httpContext.Request.Method}");
        await next.Invoke(httpContext);
        logger.LogInformation($"finish requst:{httpContext.Request.Method}");
    }
}
public static partial class ExtentionMidelware
{
    public static IApplicationBuilder UseLoger(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<Loger>();
    }
}