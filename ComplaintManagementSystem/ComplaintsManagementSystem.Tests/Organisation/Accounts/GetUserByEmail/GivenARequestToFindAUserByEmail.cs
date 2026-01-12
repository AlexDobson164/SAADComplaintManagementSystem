using static NHibernate.Engine.Query.CallableParser;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToFindAUserByEmail
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    GetUserByEmailResponse userInfo;
    Guid userReference;
    Guid businessReference = Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9");
    [SetUp]
    public async Task SetUp()
    {
        var response = await accountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = "findwithemail@email.com",
            HashedPassword = "hashPassword",
            Salt = "passwordSalt",
            BusinessReferece = businessReference,
            FirstName = "John",
            LastName = "Doe",
            Role = RolesEnum.HelpDeskAgent
        }, CancellationToken.None);

        userReference = response.UserReference;

        userInfo = await accountsHostedService.GetUserByEmail(new GetUserByEmailRequest
        {
            Email = "findwithemail@email.com"
        },CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectInformationIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(userInfo.Reference, Is.EqualTo(userReference));
            Assert.That(userInfo.Email, Is.EqualTo("findwithemail@email.com"));
            Assert.That(userInfo.BusinessReference, Is.EqualTo(businessReference));
            Assert.That(userInfo.FirstName, Is.EqualTo("John"));
            Assert.That(userInfo.LastName, Is.EqualTo("Doe"));
            Assert.That(userInfo.Role, Is.EqualTo(RolesEnum.HelpDeskAgent));
        });
    }
}
