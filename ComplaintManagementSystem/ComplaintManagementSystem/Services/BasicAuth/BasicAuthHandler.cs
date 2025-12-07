using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
// I follopwed this video on how to implement basic auth as i haven't done it before - https://www.youtube.com/watch?v=rGfxURLQp7o
public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorizaiton Key"));

        var authHeader = Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(AuthenticateResult.Fail("Authorization header does not start with 'Basic '"));

        string decodedAuth = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase)));

        string[] splitAuth = decodedAuth.Split(":", 2);

        if (splitAuth.Length != 2)
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header Format"));

        var userEmail = splitAuth[0];
        var userPassword = splitAuth[1];

        // auth with the db

        if (false) //auth check for db goes here
            return Task.FromResult(AuthenticateResult.Fail("Username or Password is incorrect"));

        var client = new BasicAuthClient
        {
            AuthenticationType = "Basic",
            IsAuthenticated = true,
            Name = userEmail,
        };

        var claimsPrinciple = new ClaimsPrincipal(new ClaimsIdentity(client, new[]
        {
            new Claim(ClaimTypes.Email, userEmail),
            new Claim(ClaimTypes.Role, "test"), // will need to populate this from the enum
            //might want to add some more later
        }));
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrinciple, Scheme.Name)));
    }
}
