using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToCreateANewUserInAnOrganisationThatDoesntExist
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    CreateAccountResponse response;
    [SetUp]
    public async Task SetUp()
    { 
        response = await accountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = "used@email.com",
            HashedPassword = "hashPassword",
            Salt = "passwordSalt",
            BusinessReferece = Guid.NewGuid(),
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
            Assert.That(response.ErrorMessages.Contains("Business does not exist"), Is.True);
        });
    }
}
