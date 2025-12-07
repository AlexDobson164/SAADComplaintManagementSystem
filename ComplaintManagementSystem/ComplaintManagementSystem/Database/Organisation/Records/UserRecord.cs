using FluentNHibernate.Mapping;

public class UserRecord
{
    public virtual int id { get; set; }
    public virtual Guid reference { get; set; }
    public virtual string email { get; set; }
    public virtual string password { get; set; }
    public virtual string salt { get; set; }
    public virtual Guid business_reference { get; set; }
    public virtual string first_name { get; set; }
    public virtual string last_name { get; set; }
    public virtual RolesEnum role { get; set; }
}
public class UserMapping : ClassMap<UserRecord>
{
    public UserMapping()
    {
        Table("organisation.\"user\"");
        Id(x => x.id)
             .Column("id")
             .GeneratedBy.Sequence("organisation.user_id_seq");
        Map(x => x.reference);
        Map(x => x.email);
        Map(x => x.password);
        Map(x => x.salt);
        Map(x => x.business_reference);
        Map(x => x.first_name);
        Map(x => x.last_name);
        Map(x => x.role)
            .CustomType<RolesEnum>();
    }
}
