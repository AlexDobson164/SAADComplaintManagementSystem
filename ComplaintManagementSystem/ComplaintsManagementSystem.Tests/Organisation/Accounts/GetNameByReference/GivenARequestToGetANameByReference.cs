using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToGetANameByReference
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    string name;
    [SetUp]
    public async Task SetUp()
    {
        Guid businessReference = Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9");
        var response = await accountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = "username@email.com",
            HashedPassword = "hashPassword",
            Salt = "passwordSalt",
            BusinessReferece = businessReference,
            FirstName = "John",
            LastName = "Doe",
            Role = RolesEnum.HelpDeskAgent
        }, CancellationToken.None);

        var usernameResponse = await accountsHostedService.GetNameByUserRef(new GetNameByUserReferenceRequest
        {
            UserReference = response.UserReference
        }, CancellationToken.None);
        name = usernameResponse.Name;
    }
    [Test]
    public async Task ThenTheCorrectNameIsReturned()
    {
        Assert.That(name, Is.EqualTo("John Doe"));
    }

}
