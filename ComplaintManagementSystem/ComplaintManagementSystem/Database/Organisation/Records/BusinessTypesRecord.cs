using FluentNHibernate.Mapping;

public class BusinessTypesRecord
{
    public virtual int id { get; set; } 
    public virtual string business_type { get; set; }
}
public class BusinessTypesMapping : ClassMap<BusinessTypesRecord>
{
    public BusinessTypesMapping() 
    {
        Table("organisation.business_types");
        Id(x => x.id);
        Map(x => x.business_type);
    }
}