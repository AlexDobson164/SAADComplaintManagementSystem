using Microsoft.AspNetCore.Identity;
using NHibernate.Util;

public static class UserTable
{
    public static bool IsEmailInUse(string email)
    {
        List<UserRecord> rows = new();
        using (var session = DatabaseConnection.GetSession())
        {
            rows = session.Query<UserRecord>()
                .Where(x => x.email == email)
                .ToList();
        }
        if (rows.Count == 1)
            return true;
        return false;
    }
    public static void SaveNewUser(User user, string hashedPassword, string salt)
    {
        using (var session = DatabaseConnection.GetSession())
        //using (var transaction = session.BeginTransaction())
        {
            session.BeginTransaction();
            session.Save(new UserRecord
            {
                reference = Guid.NewGuid(),
                email = user.Email,
                password = hashedPassword,
                salt = salt,
                business_reference = user.BusinessReference,
                first_name = user.FirstName,
                last_name = user.LastName,
                role = user.Role
            });
            session.Transaction.Commit();
        }
    }
    public static PasswordAndSaltDto GetPasswordAndSalt(string email)
    {
       
        using (var session = DatabaseConnection.GetSession())
        {
            return session.Query<UserRecord>()
                .Where(x => x.email == email)
                .Select(x => new PasswordAndSaltDto
                {
                    Password = x.password,
                    Salt = x.salt
                })
                .SingleOrDefault();
        };
    }
    public static User GetUserByEmail(string email)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            return session.Query<UserRecord>()
                .Where(x => x.email == email)
                .Select(x => new User
                {
                    Reference = x.reference,
                    Email = x.email,
                    BusinessReference = x.business_reference,
                    FirstName = x.first_name,
                    LastName = x.last_name,
                    Role = x.role
                })
                .FirstOrDefault();
        }
    }
}
public class PasswordAndSaltDto
{
    public string Password { get; set; }
    public string Salt { get; set; }
}