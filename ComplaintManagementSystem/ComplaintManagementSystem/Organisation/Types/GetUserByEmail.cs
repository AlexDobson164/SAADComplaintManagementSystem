 public class GetUserByEmailRequest
{
    public string Email { get; set; }
}
public class GetUserByEmailResponse
{
    public bool IsSuccessful {get; set;}
    public int ErrorCode { get; set;}
    public string ErrorMessage { get; set;}
    public Guid Reference { get; set; }
    public string Email { get; set; }
    public Guid BusinessReference { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public RolesEnum Role { get; set; }
}