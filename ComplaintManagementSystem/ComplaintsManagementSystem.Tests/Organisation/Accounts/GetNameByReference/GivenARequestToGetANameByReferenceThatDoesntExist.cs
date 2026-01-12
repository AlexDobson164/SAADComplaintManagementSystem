using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintManagementSystem.Tests.Organisation.Accounts;
[TestFixture]
public class GivenARequestToGetANameByReferenceThatDoesntExist
{
    AccountsHostedService accountsHostedService = new AccountsHostedService();
    GetNameByUserReferenceResponse response;
    [SetUp]
    public async Task SetUp()
    {


        response = await accountsHostedService.GetNameByUserRef(new GetNameByUserReferenceRequest
        {
            UserReference = Guid.NewGuid()
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectNameIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessful, Is.False);
            Assert.That(response.Error, Is.EqualTo("User not found"));
        });
    }

}
