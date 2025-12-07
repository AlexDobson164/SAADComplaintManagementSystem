public class User
{
    public Guid Reference { get; set; }
    public string Email { get; set; }
    public Guid BusinessReference { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public RolesEnum Role { get; set; }
}
