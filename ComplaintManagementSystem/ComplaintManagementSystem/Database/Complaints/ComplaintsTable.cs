using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Param;

public static class ComplaintsTable
{
    public static async Task<Guid> SaveNewComplaintRecord(CreateComplaintData data, CancellationToken cancellationToken)
    {
        Guid reference = Guid.NewGuid();

        using (var session = DatabaseConnection.GetSession())
        {
            session.BeginTransaction();
            session.Save(new ComplaintsRecord
            {
                reference = reference,
                consumer_email = data.ConsumerEmail,
                consumer_post_code = data.ConsumerPostcode,
                first_message = data.Message,
                business_reference = data.BusinessReference,
                time_opened = DateTime.UtcNow,
                is_open = true,
                last_updated = DateTime.UtcNow
            });
            await session.Transaction.CommitAsync(cancellationToken);
        }
        return reference;
    }
    public static async Task<bool> CheckIfComplaintExists(Guid complaintReference, Guid businessReference, CancellationToken cancellationToken)
    {
        List<ComplaintsRecord> rows = new();
        using (var session = DatabaseConnection.GetSession())
        {
            rows = await session.Query<ComplaintsRecord>()
                .Where(x => x.reference == complaintReference && 
                       x.business_reference == businessReference)
                .ToListAsync(cancellationToken);
        }
        if (rows.Count > 0)
            return true;
        return false;
    }
    public static async Task<List<ComplaintInformation>> SearchForComplaints(ComplaintsSearchRequest request, CancellationToken cancellationToken)
    {
        List<ComplaintInformation> complaints = new();

        using(var session = DatabaseConnection.GetSession())
        {
            var query = session.QueryOver<ComplaintsRecord>();
            
            if (!String.IsNullOrEmpty(request.ConsumerPostCode))
                query.WhereRestrictionOn(x => x.consumer_post_code)
                    .IsLike(request.ConsumerPostCode, MatchMode.Anywhere);

            if (!String.IsNullOrEmpty(request.ConsumerEmail))
                query.WhereRestrictionOn(x => x.consumer_email)
                    .IsLike(request.ConsumerEmail, MatchMode.Anywhere);

            if (!String.IsNullOrEmpty(request.FirstNote))
                query.WhereRestrictionOn(x => x.first_message)
                    .IsInsensitiveLike(request.FirstNote, MatchMode.Anywhere);

            if (request.ComplaintReference != null)
                query.Where(x => x.reference == request.ComplaintReference);

            if (request.IsOpen != null)
                query.Where(x => x.is_open == request.IsOpen);

            var result = await query.Where(x => x.business_reference == request.BusinessReference)
                .ListAsync(cancellationToken).ConfigureAwait(false);
               
            complaints = result.Select(x => new ComplaintInformation
            {
                Reference = x.reference,
                FirstMessage = x.first_message,
                TimeOpened = x.time_opened,
                LastUpdated = x.last_updated,
                IsOpen = x.is_open
            }).ToList();
        }
        return complaints;
    }
}