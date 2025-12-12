using NHibernate.Collection.Generic;

public class GetUsersByRoleRequest
{
    public Guid BusinessReference { get; set; }
    public RolesEnum Role { get; set; }
}
public class GetUserByRoleResponse
{
    public List<UserByRole> Users { get; set; }
    public bool IsSuccessful { get; set; }
    public string Error { get; set; }
}
public class UserByRole
{
    public Guid UserReference { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}