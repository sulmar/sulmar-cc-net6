namespace CC.HelpDesk.Api.Middlewares;

public class SecretKeyMiddleware
{
    private readonly RequestDelegate next;
    private readonly IConfiguration configuration;
    public SecretKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        this.next = next;
        this.configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
            if (context.Request.Headers.TryGetValue("X-Secret-Key", out var secretKey) 
                        && secretKey == configuration["SecretKey"])
                await next(context);
            else
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
    }



}
