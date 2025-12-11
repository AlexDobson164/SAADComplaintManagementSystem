// based this implementation off of the first example in this video - https://www.youtube.com/watch?v=GrJJXixjR8M
public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    private BusinessHostedService _BusinessHostedService = new BusinessHostedService();
    public ApiKeyAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        CancellationToken cancellationToken = context.RequestAborted;
        if (!context.Request.Headers.TryGetValue("x-api-key", out var apiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key Missing");
            return;
        }

        if (!Guid.TryParse(apiKey, out var parsedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        var response = await _BusinessHostedService.ValidateApiKey(new ValidateApiKeyRequest
        {
            ApiKey = parsedApiKey
        }, cancellationToken);
        if (response.BusinessReference == Guid.Empty)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        await _next(context);
    }
}
