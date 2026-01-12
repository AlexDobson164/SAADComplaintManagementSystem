using static NHibernate.Engine.Query.CallableParser;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToFindAUserByReference
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    User user;
    Guid userReference;
    Guid businessReference = Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9");
    [SetUp]
    public async Task SetUp()
    {
        var response = await accountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = "findwithreference@email.com",
            HashedPassword = "hashPassword",
            Salt = "passwordSalt",
            BusinessReferece = businessReference,
            FirstName = "John",
            LastName = "Doe",
            Role = RolesEnum.HelpDeskAgent
        }, CancellationToken.None);

        userReference = response.UserReference;

        var userInfo = await accountsHostedService.GetUserByReference(new GetUserByReferenceRequest
        {
            BusinessReference = businessReference,
            UserReference = userReference
            
        },CancellationToken.None);
        
        user = userInfo.User;
    }
    [Test]
    public async Task ThenTheCorrectInformationIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(userReference));
            Assert.That(user.Email, Is.EqualTo("findwithreference@email.com"));
            Assert.That(user.BusinessReference, Is.EqualTo(businessReference));
            Assert.That(user.FirstName, Is.EqualTo("John"));
            Assert.That(user.LastName, Is.EqualTo("Doe"));
            Assert.That(user.Role, Is.EqualTo(RolesEnum.HelpDeskAgent));
        });
    }
}
