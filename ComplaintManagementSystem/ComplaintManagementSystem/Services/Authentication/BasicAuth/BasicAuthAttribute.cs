using Microsoft.AspNetCore.Authorization;

public class BasicAuthAttribute :  AuthorizeAttribute
{
    public BasicAuthAttribute()
    {
        AuthenticationSchemes = "Basic";
    }
}