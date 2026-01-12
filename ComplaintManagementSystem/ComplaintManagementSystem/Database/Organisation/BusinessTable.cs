using NHibernate;
using NHibernate.Linq;

public static class BusinessTable
{
    public static async Task<List<BusinessRecord>> GetAllBusinessAsync(CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            return await session.Query<BusinessRecord>().ToListAsync(cancellationToken);
        }
    }
    public static async Task<bool> BusinessExistsByRef(Guid reference, CancellationToken cancellationToken)
    {
        int rowCount;
        using (var session = DatabaseConnection.GetSession())
        {
            rowCount = await session.Query<BusinessRecord>()
                .Where(x => x.reference == reference)
                .CountAsync(cancellationToken);
        }
        if (rowCount == 1)
            return true;
        return false;
    }
    public static async Task<Guid> GetBusinessRefByApiKey(Guid apiKey, CancellationToken cancellationToken)
    {
        BusinessRecord record;
        using (var session = DatabaseConnection.GetSession())
        {
            record = await session.Query<BusinessRecord>()
                .Where(x => x.api_key == apiKey)
                .FirstOrDefaultAsync(cancellationToken);
        }
        if (record == null)
            return Guid.Empty;
        return record.reference;
    }
    //this method is only here for testing
    public static async Task CreateNewTestBusiness(Guid businessReference, string businessName, BusinessTypesEnum businessType, Guid ApiKey, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            session.BeginTransaction();
            session.Save(new BusinessRecord
            {
                reference = businessReference,
                name = businessName,
                business_type = businessType,
                api_key = ApiKey,
            });
            await session.GetCurrentTransaction().CommitAsync(cancellationToken);
        }
    }
}
