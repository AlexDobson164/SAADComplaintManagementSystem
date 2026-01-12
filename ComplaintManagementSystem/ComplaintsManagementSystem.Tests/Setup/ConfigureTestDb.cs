using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

[SetUpFixture]
public class ConfigureTestDb
{
    public static PostgreSqlContainer Postgres { get; private set; }

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        Postgres = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("testdb")
            .WithUsername("test")
            .WithPassword("test")
            .Build();

        await Postgres.StartAsync();

      
        DatabaseConnection.InitTestConnection(Postgres.GetConnectionString());
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        await Postgres.DisposeAsync();
    }
}
