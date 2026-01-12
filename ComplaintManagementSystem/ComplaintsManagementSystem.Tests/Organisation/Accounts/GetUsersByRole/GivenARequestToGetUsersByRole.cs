using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToGetUsersByRole
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    GetAllUsersByRoleResponse supportEngineerResponse;
    GetAllUsersByRoleResponse managerResponse;
    [SetUp]
    public async Task SetUp()
    {
        Guid businessReference = Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9");
        await accountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = "supportEngineer1@email.com",
            HashedPassword = "hashPassword",
            Salt = "passwordSalt",
            BusinessReferece = businessReference,
            FirstName = "John",
            LastName = "Doe",
            Role = RolesEnum.SupportEngineer
        }, CancellationToken.None);
        await accountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = "supportEngineer2@email.com",
            HashedPassword = "hashPassword",
            Salt = "passwordSalt",
            BusinessReferece = businessReference,
            FirstName = "John",
            LastName = "Doe",
            Role = RolesEnum.SupportEngineer
        }, CancellationToken.None);
        await accountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = "manager@email.com",
            HashedPassword = "hashPassword",
            Salt = "passwordSalt",
            BusinessReferece = businessReference,
            FirstName = "Manager",
            LastName = "Doe",
            Role = RolesEnum.HelpDeskManager
        }, CancellationToken.None);

        supportEngineerResponse = await accountsHostedService.GetAllUsersByRole(new GetAllUsersByRoleRequest
        {
            Role = RolesEnum.SupportEngineer,
            BusinessReference = businessReference
        }, CancellationToken.None);
        managerResponse = await accountsHostedService.GetAllUsersByRole(new GetAllUsersByRoleRequest
        {
            Role = RolesEnum.HelpDeskManager,
            BusinessReference = businessReference
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectNumberOfUsersAreReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(supportEngineerResponse.Users.Count, Is.EqualTo(2));
            Assert.That(managerResponse.Users.Count, Is.EqualTo(1));
        });
    }
    [Test]
    public async Task ThenTheReturnedInfoIsCorrect()
    {
        var manager = managerResponse.Users.FirstOrDefault();
        Assert.Multiple(() =>
        {
            Assert.That(manager.Email, Is.EqualTo("manager@email.com"));
            Assert.That(manager.Name, Is.EqualTo("Manager Doe"));
        });
    }


}
