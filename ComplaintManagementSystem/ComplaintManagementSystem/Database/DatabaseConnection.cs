using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System.Configuration;

public class DatabaseConnection
{
    private static ISessionFactory _sessionFactory;
    private static ISessionFactory SessionFactory
    {
        get
        {
            if (_sessionFactory == null)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                

                _sessionFactory = Fluently.Configure()
                    .Database(PostgreSQLConfiguration.Standard
                    .ConnectionString(c => c
                    .Host("localhost")
                    .Port(5432)
                    .Database("complaintmanagementsystem")
                    .Username(config["ConnectionStrings:DBUsername"])
                    .Password(config["ConnectionStrings:DBPassword"])))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                    .BuildSessionFactory();
            }
            return _sessionFactory;
        }
    }

    public static NHibernate.ISession GetSession()
    {
        return SessionFactory.OpenSession();
    }
}