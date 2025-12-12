using Microsoft.AspNetCore.Identity;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

public static class UserTable
{
    public static async Task<bool> IsEmailInUse(string email, CancellationToken cancellationToken)
    {
        List<UserRecord> rows = new();
        using (var session = DatabaseConnection.GetSession())
        {
            rows = await session.Query<UserRecord>()
                .Where(x => x.email == email)
                .ToListAsync(cancellationToken);
        }
        if (rows.Count == 1)
            return true;
        return false;
    }
    public static async Task SaveNewUser(User user, string hashedPassword, string salt, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
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
            await session.GetCurrentTransaction().CommitAsync(cancellationToken);
        }
    }
    public static async Task<PasswordAndSaltDto> GetPasswordAndSalt(string email, CancellationToken cancellationToken)
    {
       
        using (var session = DatabaseConnection.GetSession())
        {
            return await session.Query<UserRecord>()
                .Where(x => x.email == email)
                .Select(x => new PasswordAndSaltDto
                {
                    Password = x.password,
                    Salt = x.salt
                })
                .FirstOrDefaultAsync(cancellationToken);
        };
    }
    public static async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            return await session.Query<UserRecord>()
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
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
public class PasswordAndSaltDto
{
    public string Password { get; set; }
    public string Salt { get; set; }
}