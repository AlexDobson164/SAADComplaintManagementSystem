using Microsoft.AspNetCore.Identity;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using Org.BouncyCastle.Asn1.Mozilla;

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
                reference = user.Reference,
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
    public static async Task<string> GetNameByReference(Guid userReference, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var record = await session.Query<UserRecord>()
                .Where(x => x.reference == userReference)
                .FirstOrDefaultAsync(cancellationToken);
            if (record == null)
                return "";
            return $"{record.first_name} {record.last_name}";
        }
    }

    public static async Task<GetUserByRoleResponse> GetUsersByRole(GetUsersByRoleRequest request, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var results = await session.Query<UserRecord>()
                .Where(x => x.business_reference == request.BusinessReference)
                .Where(x => x.role == request.Role)
                .ToListAsync(cancellationToken);
            return new GetUserByRoleResponse
            {
                IsSuccessful = true,
                Users = results.ConvertAll(x => new UserByRole
                {
                    UserReference = x.reference,
                    Email = x.email,
                    Name = $"{x.first_name} {x.last_name}"
                })
            };
        }
    }

    public static async Task<User> GetUserByReference(Guid userReference, Guid BusinessReference, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var record = await session.Query<UserRecord>()
                .Where(x => x.reference == userReference)
                .Where(x => x.business_reference == BusinessReference)
                .FirstOrDefaultAsync(cancellationToken);

            if (record == null)
                return null;

            return new User
            {
                Reference = record.reference,
                Email = record.email,
                BusinessReference = record.business_reference,
                FirstName = record.first_name,
                LastName = record.last_name,
                Role = record.role,
            };
        }
    }
}
