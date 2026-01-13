using Microsoft.AspNetCore.Mvc;
using Testcontainers.PostgreSql;

namespace ComplaintManagementSystem.Services;
[ApiController]
[Route("[controller]")]
public class ConfigureTestDatabaseConstroler : ControllerBase
{
    HashHostedService hashHostedService = new HashHostedService();
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    public static PostgreSqlContainer Postgres { get; private set; }
    [HttpPost("ConfigureTestDatabase", Name = "ConfigureTestDatabase")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> ConfigureTestDatabase(string password, CancellationToken cancellationToken)
    {
        if (hashHostedService.HashPassword(new HashPasswordRequest
        {
            Password = password,
            Salt = "238742357824"
        }).HashedPassword == "c7562201845892e28686af9f4e7e53fa16308bc2d26c3797e4dcb8e57e40f345")
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
            await BusinessTable.CreateNewTestBusiness(Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9"), "Test Business", BusinessTypesEnum.Bank, Guid.Parse("618FF1F2-0C8B-4EA5-ABA5-3E1C63134EE8"), cancellationToken);
            await accountsHostedService.CreateAccount(new CreateAccountRequest
            {
                Email = "systemadmin@test.com",
                HashedPassword = "c3ae1f8ee488209d26b5086736a78325928c8da0a4cd7b5f6844c8e0dfc51653",
                Salt = "QH8/PMEtg1yDQUnMGgcSSA",
                BusinessReferece = Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9"),
                FirstName = "Test",
                LastName = "SystemAdmin",
                Role = RolesEnum.SystemAdmin

            },cancellationToken); ; 
            return Results.Ok();
        }
        return Results.Unauthorized();
    }
}
