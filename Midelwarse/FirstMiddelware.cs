namespace Middelware;
//שאלה למורה: איך אפשר לדעת איזה ניים-ספיס לעשות?
public class FirstMiddelware
{
    private RequestDelegate next;
    public FirstMiddelware(RequestDelegate next)
    {
        this.next =next;
    }

    public async Task Invoke(HttpContext httpContext){
        try
        {
            await next.Invoke(httpContext);
        }
        catch (System.Exception e)
        {
           await httpContext.Response.WriteAsync(""+e.GetHashCode);
        }
    }
}
public static partial class ExtentionMidelware{
    public static IApplicationBuilder UseFirstMiddelware(this IApplicationBuilder builder){
        return builder.UseMiddleware<FirstMiddelware>();
    }
}