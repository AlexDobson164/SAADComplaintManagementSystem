using FluentNHibernate.Mapping;

public class ComplaintAssignmentRecord
{
    public virtual int id { get; set; }
    public virtual Guid complaint_reference { get; set; }
    public virtual Guid user_reference { get; set; }
}
public class ComplaintAssingmentMapping : ClassMap<ComplaintAssignmentRecord>
{
    public ComplaintAssingmentMapping()
    {
        Table("complaints.complaint_assignment");
        Id(x => x.id)
            .Column("id")
            .GeneratedBy.Identity();
        Map(x => x.complaint_reference);
        Map(x => x.user_reference);
    }
}

