using Microsoft.AspNetCore.Http;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToCloseAComplaintWithoutAReasonByAUser
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    UserCloseComplaintResponse closeResponse;
    Guid complaintReference;
    [SetUp]
    public async Task Setup()
    {
        var createResponse = await complaintsHostedService.CreateComplaint(new CreateComplaintRequest
        {
            BusinessReference = Guid.Empty,
            FirstMessage = "this is a complaint to be closed by the consumer",
            ConsumerEmail = "email@email.com",
            ConsumerPostcode = "S1 4RA"
        }, CancellationToken.None);

        complaintReference = createResponse.ComplaintReference.Value;

        closeResponse = await complaintsHostedService.UserCloseComplaint(new UserCloseComplaintRequest
        {
            BusinessReference = Guid.Empty,
            ComplaintReference = complaintReference,
            ConsumerEmail = "email@email.com",
            ConsumerPostcode = "S1 4RA",
            Feedback = "This is feedback by the consumer when the user closes the complaint.",
            Reason = ""
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectErrorIsReturned()
    {
        var complaintInfo = await complaintsHostedService.ViewComplaint(new ViewComplaintRequest
        {
            ComplaintReference = complaintReference,
            BusinessReference = Guid.Empty,
            GetPrivate = true
        }, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(complaintInfo.IsOpen, Is.True);
            Assert.That(closeResponse.IsSuccessful, Is.False);
            Assert.That(closeResponse.ErrorCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(closeResponse.ErrorMessage, Is.EqualTo("Did not provide a reason or days since last update has not reached 14 yet"));
        });
    }
}
