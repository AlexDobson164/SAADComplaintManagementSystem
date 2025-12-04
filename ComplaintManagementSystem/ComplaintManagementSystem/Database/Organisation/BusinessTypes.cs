public class BusinessTypes
{
    public BusinessTypesRecord[] GetBusinessTypes()
    {
        using (var session = DatabaseConnection.GetSession())
        {
            return session.Query<BusinessTypesRecord>().ToArray();
        }
    }
}
