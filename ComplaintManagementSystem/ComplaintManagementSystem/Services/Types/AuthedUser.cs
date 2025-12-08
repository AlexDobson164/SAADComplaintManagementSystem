using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

public class AuthedUser
{
    public Guid Reference { get; set; }
    public string Email { get; set; }
    public Guid BusinessReference { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public RolesEnum Role { get; set; }

    public AuthedUser(ClaimsPrincipal claim)
    {
        Reference = Guid.Parse(claim.FindFirst("UserReference")?.Value);
        Email = claim.FindFirst(ClaimTypes.Email)?.Value;
        BusinessReference = Guid.Parse(claim.FindFirst("BusinessReference")?.Value);
        FirstName = claim.FindFirst(ClaimTypes.GivenName)?.Value;
        LastName = claim.FindFirst(ClaimTypes.Surname)?.Value;
        Role = Enum.Parse<RolesEnum>(claim.FindFirst(ClaimTypes.Role)?.Value);
    }
}
