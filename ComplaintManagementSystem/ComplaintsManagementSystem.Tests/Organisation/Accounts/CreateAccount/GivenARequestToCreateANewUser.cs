using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToCreateANewUser
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    User user;
    [SetUp]
    public async Task SetUp()
    {
        Guid businessReference = Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9");
        var response = await accountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = "newaccount@email.com",
            HashedPassword = "hashPassword",
            Salt = "passwordSalt",
            BusinessReferece = businessReference,
            FirstName = "John",
            LastName = "Doe",
            Role = RolesEnum.HelpDeskAgent
        }, CancellationToken.None);

        var getUserResponse = await accountsHostedService.GetUserByReference(new GetUserByReferenceRequest
        {
            BusinessReference = businessReference,
            UserReference = response.UserReference
        }, CancellationToken.None);
        user = getUserResponse.User;
    }
    [Test]
    public async Task ThenTheCorrectInformationIsSavedAndRetrieved()
    {
        Assert.Multiple(() =>
        {
            Assert.That(user.Email, Is.EqualTo("newaccount@email.com"));
            Assert.That(user.FirstName, Is.EqualTo("John"));
            Assert.That(user.LastName, Is.EqualTo("Doe"));
            Assert.That(user.Role, Is.EqualTo(RolesEnum.HelpDeskAgent));
        });
    }
}
