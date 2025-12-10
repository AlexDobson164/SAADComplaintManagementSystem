namespace ComplaintManagementSystem.Database.Complaints
{
    public static class ComplaintsTable
    {
        public static Guid SaveNewComplaintRecord(CreateComplaintData data)
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
                session.Transaction.Commit();
            }
            return reference;
        }
    }
}
