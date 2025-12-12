public class GetAllUsersByRoleRequest
{
    public RolesEnum Role { get; set; }
    public Guid BusinessReference { get; set; }
}
public class GetAllUsersByRoleResponse
{
    public List<UserByRoleInfo> Users { get; set; }
    public bool IsSuccessful { get; set; }
    public string Error { get; set; }
}
public class UserByRoleInfo
{
    public Guid UserReference { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}