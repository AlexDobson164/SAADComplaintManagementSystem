using FluentNHibernate.Mapping;

public class BusinessRecord
{
    public virtual int id { get; set; }
    public virtual Guid reference {  get; set; }
    public virtual string name { get; set; }
    public virtual BusinessTypesEnum business_type { get; set; }
    public virtual Guid api_key { get; set; }
}
public class BusinessMapping : ClassMap<BusinessRecord>
{ 
    public BusinessMapping()
    {
        Table("organisation.business");
        Id(x => x.id);
        Map(x => x.reference);
        Map(x => x.name);
        Map(x => x.business_type)
            .CustomType<BusinessTypesEnum>();
        Map(x => x.api_key);
    }
}