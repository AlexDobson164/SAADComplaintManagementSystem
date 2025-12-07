public class CreateAccountRequest
{
    public string Email {  get; set; }
    public string HashedPassword { get; set; }
    public string Salt { get; set; }
    public Guid BusinessReferece { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public RolesEnum Role { get; set; }
}
public class CreateAccountResponse
{
    public bool IsSuccessful { get; set; }
    public string[]? ErrorMessages { get; set; }
}
