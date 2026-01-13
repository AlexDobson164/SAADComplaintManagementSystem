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
    AccountsHostedService accountsHostedService = new AccountsHostedService();

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

        //when split out into a microservice, this would only be in the organisation component.
        await BusinessTable.CreateNewTestBusiness(Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9"), "Test Business", BusinessTypesEnum.Bank, Guid.Parse("618FF1F2-0C8B-4EA5-ABA5-3E1C63134EE8"), CancellationToken.None);
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        await Postgres.DisposeAsync();
    }
}
