namespace ComplaintManagementSystem.Tests.Organisation.Business;
[TestFixture]
public class GivenARequestToValidateAnApiKeyThatDoesNotExist
{
    BusinessHostedService businessHostedService = new BusinessHostedService();
    ValidateApiKeyResponse response;
    [SetUp]
    public async Task SetUp()
    {
        response = await businessHostedService.ValidateApiKey(new ValidateApiKeyRequest
        {
            ApiKey = Guid.NewGuid()
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheAnEmptyBusinessReferenceIsReturned()
    {
        Assert.That(response.BusinessReference, Is.EqualTo(Guid.Empty));
    }
}
