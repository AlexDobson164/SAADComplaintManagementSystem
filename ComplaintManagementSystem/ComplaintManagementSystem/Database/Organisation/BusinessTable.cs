public static class BusinessTable
{
    public static BusinessRecord[] GetAllBusiness()
    {
        using (var session = DatabaseConnection.GetSession())
        {
            return session.Query<BusinessRecord>().ToArray();
        }
    }
    public static bool BusinessExistsByRef(Guid reference)
    {
        int rowCount;
        using (var session = DatabaseConnection.GetSession())
        {
            rowCount = session.Query<BusinessRecord>()
                .Where(x => x.reference == reference)
                .Count();
        }
        if (rowCount == 1)
            return true;
        return false;
    }

    public static Guid GetBusinessRefByApiKey(Guid apiKey)
    {
        BusinessRecord record;
        using (var session = DatabaseConnection.GetSession())
        {
            record = session.Query<BusinessRecord>()
                .Where(x => x.api_key == apiKey)
                .FirstOrDefault();
        }
        if (record == null)
            return Guid.Empty;
        return record.reference;
    }
}
