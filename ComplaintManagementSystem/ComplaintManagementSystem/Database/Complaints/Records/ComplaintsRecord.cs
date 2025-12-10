using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;

public class ComplaintsRecord
{
    public virtual int id { get; set; }
    public virtual Guid reference { get; set; }
    public virtual string consumer_email { get; set; }
    public virtual string consumer_post_code { get; set; }
    public virtual string first_message { get; set; }
    public virtual Guid business_reference { get; set; }
    public virtual DateTime time_opened { get; set; }
    public virtual bool is_open { get; set; }
    public virtual Guid? closed_by { get; set; }
    public virtual string? closed_reason { get; set; }
    public virtual DateTime last_updated { get; set; }
}
public class ComplaintsMapping : ClassMap<ComplaintsRecord>
{
    ComplaintsMapping()
    {
        Table("complaints.complaints");
        Id(x => x.id)
            .Column("id")
            .GeneratedBy.Sequence("complaints.complaints_id_seq"); ;
        Map(x => x.reference);
        Map(x => x.consumer_email);
        Map(x => x.consumer_post_code);
        Map(x => x.first_message);
        Map(x => x.business_reference);
        Map(x => x.time_opened);
        Map(x => x.is_open);
        Map(x => x.closed_by);
        Map(x => x.closed_reason);
        Map(x => x.last_updated);
    }
}