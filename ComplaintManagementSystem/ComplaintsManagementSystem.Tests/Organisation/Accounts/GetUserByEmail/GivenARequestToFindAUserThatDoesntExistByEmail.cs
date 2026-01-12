using Microsoft.AspNetCore.Http;
using static NHibernate.Engine.Query.CallableParser;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToFindAUserThatDoesntExistByEmail
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    GetUserByEmailResponse userInfo;
    [SetUp]
    public async Task SetUp()
    {
        userInfo = await accountsHostedService.GetUserByEmail(new GetUserByEmailRequest
        {
            Email = "none@email.com"
        },CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectErrorIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(userInfo.IsSuccessful, Is.False);
            Assert.That(userInfo.ErrorCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(userInfo.ErrorMessage, Is.EqualTo("user not found"));
        });
    }
}
