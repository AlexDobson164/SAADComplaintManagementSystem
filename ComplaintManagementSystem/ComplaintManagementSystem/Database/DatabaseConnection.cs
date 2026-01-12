using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
using Npgsql;

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

    public static void InitTestConnection(string connectionString)
    {
        if (_sessionFactory != null)
            _sessionFactory.Dispose();

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "CREATE SCHEMA IF NOT EXISTS organisation; CREATE SCHEMA IF NOT EXISTS complaints";
            cmd.ExecuteNonQuery();
        }

        _sessionFactory = Fluently.Configure()
            .Database(PostgreSQLConfiguration.Standard
                .Dialect<PostgreSQL82Dialect>()
                .ConnectionString(connectionString))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
            .ExposeConfiguration(config =>
                new NHibernate.Tool.hbm2ddl.SchemaExport(config).Create(false, true))
            .BuildSessionFactory();
    }

    public static NHibernate.ISession GetSession()
    {
        return SessionFactory.OpenSession();
    }
}