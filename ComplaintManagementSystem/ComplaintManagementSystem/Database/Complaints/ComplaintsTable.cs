using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

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
            await session.GetCurrentTransaction().CommitAsync(cancellationToken);
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
    public static async Task LastUpdated(Guid complaintReference, Guid BusinessReference, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var record = await session.Query<ComplaintsRecord>()
                .Where(x => x.reference == complaintReference)
                .Where(x => x.business_reference == BusinessReference)
                .FirstOrDefaultAsync(cancellationToken);
            
            record.last_updated = DateTime.UtcNow;

            session.BeginTransaction();
            await session.UpdateAsync(record);
            await session.GetCurrentTransaction().CommitAsync(cancellationToken);
        }
    }

    public static async Task<bool> CloseComplaint(CloseComplaint request, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var record = await session.Query<ComplaintsRecord>()
                .Where(x => x.reference == request.ComplaintReference)
                .Where(x => x.business_reference == request.BusinessReference)
                .Where(x => x.consumer_email == request.ConsumerEmail)
                .Where(x => x.consumer_post_code == request.ConsumerPostcode)
                .FirstOrDefaultAsync(cancellationToken);

            if (record == null)
                return false;

            record.last_updated = DateTime.UtcNow;
            record.closed_by = request.UserReference;
            record.is_open = false;
            record.closed_reason = request.CloseReason;

            session.BeginTransaction();
            await session.UpdateAsync(record);
            await session.GetCurrentTransaction().CommitAsync(cancellationToken);
        }
        return true;
    }
    public static async Task<DateTime> CheckWhenLastUpdated(Guid complaintReference, Guid businessReference, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var record = await session.Query<ComplaintsRecord>()
                .Where(x => x.reference ==  complaintReference)
                .Where(x => x.business_reference == businessReference)
                .FirstOrDefaultAsync(cancellationToken);
            return record.last_updated;
        }
    }
    public static async Task<CloseComplaintWithoutConsumerInfoResponse> CloseComplaintWithoutConsumerInfo(CloseComplaintWithoutConsumerInfoRequest request, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var record = await session.Query<ComplaintsRecord>()
                .Where(x => x.reference == request.ComplaintReference)
                .Where(x => x.business_reference == request.BusinessReference)
                .FirstOrDefaultAsync(cancellationToken);

            if (record == null)
                return new CloseComplaintWithoutConsumerInfoResponse
                {
                    IsSuccessful = false,
                    ConsumerEmail = record.consumer_email
                };

            record.last_updated = DateTime.UtcNow;
            record.closed_by = request.UserReference;
            record.is_open = false;
            record.closed_reason = request.CloseReason;

            session.BeginTransaction();
            await session.UpdateAsync(record);
            await session.GetCurrentTransaction().CommitAsync(cancellationToken);

            return new CloseComplaintWithoutConsumerInfoResponse
            {
                IsSuccessful = true,
                ConsumerEmail = record.consumer_email
            };
        }
    }
    public static async Task<bool> IsComplaintOpen(Guid complaintReference, Guid businessReference, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var response = await session.Query<ComplaintsRecord>()
                .Where(x => x.reference == complaintReference)
                .Where(x => x.business_reference == businessReference)
                .FirstOrDefaultAsync(cancellationToken);
            return response.is_open;
        }
    }
}