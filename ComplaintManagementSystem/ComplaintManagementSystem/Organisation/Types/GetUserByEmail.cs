 public class GetUserByEmailRequest
{
    public string Email { get; set; }
}
public class GetUserByEmailResponse
{
    public Guid Reference { get; set; }
    public string Email { get; set; }
    public Guid BusinessReference { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public RolesEnum Role { get; set; }
}