using System.Security.Cryptography.X509Certificates;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToGetAuthInfoByEmail
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    AuthInfoResponse authInfo;
    [SetUp]
    public async Task SetUp()
    {
        Guid businessReference = Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9");
        var createResponse = await accountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = "authinfo@email.com",
            HashedPassword = "hashPassword",
            Salt = "passwordSalt",
            BusinessReferece = businessReference,
            FirstName = "John",
            LastName = "Doe",
            Role = RolesEnum.HelpDeskAgent
        }, CancellationToken.None);

        authInfo = await accountsHostedService.GetAuthInfoByEmail(new AuthInfoRequest
        {
            Email = "authinfo@email.com"
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectInfoIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(authInfo.HashedPassword, Is.EqualTo("hashPassword"));
            Assert.That(authInfo.Salt, Is.EqualTo("passwordSalt"));
        });
    }
}
