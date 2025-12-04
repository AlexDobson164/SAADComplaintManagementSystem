using FluentNHibernate.Mapping;
using NHibernate.Linq.Functions;

public class BusinessTypesTable
{
    public BusinessTypeInfo[] GetBusinessTypes()
    {
        using(var session = DatabaseConnection.GetSession())
        {
            //  return session.Get<BusinessType>();
            return session.Query<BusinessTypeInfo>().ToArray();
        }
    }
    
}

public class BusinessTypeInfo
{
    public virtual int id { get; set; } 
    public virtual string business_type { get; set; }
}
// need to split this out into a record
public class BusinessTypesMapping : ClassMap<BusinessTypeInfo>
{
    public BusinessTypesMapping() 
    {
        Table("organisation.business_types");
        Id(x => x.id);
        Map(x => x.business_type);
    }
}