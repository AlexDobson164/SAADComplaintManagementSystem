namespace ComplaintManagementSystem.Tests.Organisation.Business;
[TestFixture]
public class GivenARequestToValidateAnApiKeyThatIsCorrect
{
    BusinessHostedService businessHostedService = new BusinessHostedService();
    ValidateApiKeyResponse response;
    [SetUp]
    public async Task SetUp()
    {
        response = await businessHostedService.ValidateApiKey(new ValidateApiKeyRequest
        {
            ApiKey = Guid.Parse("618FF1F2-0C8B-4EA5-ABA5-3E1C63134EE8")
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectBusinessReferenceIsReturned()
    {
        Assert.That(response.BusinessReference, Is.EqualTo(Guid.Parse("C83BAB21-F173-475B-A9DA-F82C91588CB9")));
    }
}
