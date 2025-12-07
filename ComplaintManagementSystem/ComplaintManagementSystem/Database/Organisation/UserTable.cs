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
}