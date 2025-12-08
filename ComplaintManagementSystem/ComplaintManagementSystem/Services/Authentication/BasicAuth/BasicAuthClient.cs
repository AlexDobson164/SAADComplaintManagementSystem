using System.Security.Principal;

public class BasicAuthClient : IIdentity
{
    public string? AuthenticationType { get; set; }
    public bool IsAuthenticated { get; set; }
    public string? Name { get; set; }
}