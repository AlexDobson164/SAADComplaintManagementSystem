namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenThatARequestToCreateAComplaintWithAnInvalidPostcode
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    CreateComplaintResponse response;

    [SetUp]
    public async Task SetUpAsync()
    {
        var request = new CreateComplaintRequest
        {
            BusinessReference = Guid.Empty,
            FirstMessage = "note",
            ConsumerEmail = "email@email.com",
            ConsumerPostcode = "invalid postcode"
        };
        response = await complaintsHostedService.CreateComplaint(request, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectErrorIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessful, Is.False);
            Assert.That(response.Errors.Contains("Invalid Postcode"), Is.True);
        });
    }
}