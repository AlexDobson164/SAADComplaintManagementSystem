namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenThatARequestToCreateAComplaintWithAnInvalidEmail
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
            ConsumerEmail = "invalid email",
            ConsumerPostcode = "S1 4RA"
        };
        response = await complaintsHostedService.CreateComplaint(request, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectErrorIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessful, Is.False);
            Assert.That(response.Errors.Contains("Invalid Email"), Is.True);
        });
    }
}