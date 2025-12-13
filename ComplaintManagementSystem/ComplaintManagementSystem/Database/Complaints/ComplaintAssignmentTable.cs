using FluentNHibernate.Utils;
using NHibernate;
using NHibernate.Linq;

public static class ComplaintAssignmentTable
{
    public static async Task<bool> IsUserAssigned(Guid userReference, Guid complaintReference, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var response = await session.Query<ComplaintAssignmentRecord>()
                .Where(x => x.user_reference == userReference)
                .Where(x => x.complaint_reference == complaintReference)
                .CountAsync(cancellationToken);

            if (response > 0)
                return true;
            return false;
        }
    }

    public static async Task AssignUser(Guid userReference, Guid complaintReference, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            session.BeginTransaction();
            session.Save(new ComplaintAssignmentRecord
            {
                user_reference = userReference,
                complaint_reference = complaintReference
            });
            await session.GetCurrentTransaction().CommitAsync(cancellationToken);
        }
    }

    public static async Task UnassignUser(Guid userReference, Guid complaintReference, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var record = await session.Query<ComplaintAssignmentRecord>()
                .Where(x => x.user_reference == userReference)
                .Where(x => x.complaint_reference == complaintReference)
                .FirstOrDefaultAsync(cancellationToken);

            session.BeginTransaction();
            session.DeleteAsync(record);
            await session.GetCurrentTransaction().CommitAsync(cancellationToken);
        }
    }
    public static async Task<List<Guid>> GetAllComplaintReferencesForUserRef(Guid userReference, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            return await session.Query<ComplaintAssignmentRecord>()
                .Where(x => x.user_reference == userReference)
                .Select(x => x.complaint_reference)
                .ToListAsync(cancellationToken);
        }
    }
}