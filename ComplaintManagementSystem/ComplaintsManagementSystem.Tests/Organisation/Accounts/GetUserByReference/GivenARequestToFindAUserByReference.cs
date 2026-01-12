using Docker.DotNet.Models;
using Microsoft.AspNetCore.Http;
using static NHibernate.Engine.Query.CallableParser;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToFindAUserThatDoesntExistByReference
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    GetUserByReferenceResponse userInfo;
    Guid userReference;
    Guid businessReference = Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9");
    [SetUp]
    public async Task SetUp()
    {


        userInfo = await accountsHostedService.GetUserByReference(new GetUserByReferenceRequest
        {
            BusinessReference = businessReference,
            UserReference = Guid.NewGuid()
        }, CancellationToken.None);
    }
    [Test]
    public async Task TheCorrectErrorIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(userInfo.IsSuccessful, Is.False);
            Assert.That(userInfo.Error, Is.EqualTo("Requested User does not exist"));
        });
    }
}
