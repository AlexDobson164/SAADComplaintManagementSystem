using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToCreateANewUserWithAnInvalidEmail
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    CreateAccountResponse response;
    [SetUp]
    public async Task SetUp()
    {
        Guid businessReference = Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9");

        response = await accountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = "invalid email",
            HashedPassword = "hashPassword",
            Salt = "passwordSalt",
            BusinessReferece = businessReference,
            FirstName = "John",
            LastName = "Doe",
            Role = RolesEnum.HelpDeskAgent
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectErrorIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessful, Is.False);
            Assert.That(response.ErrorMessages.Contains("Invalid Email"), Is.True);
        });
    }
}
