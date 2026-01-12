using Microsoft.AspNetCore.Http;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToCloseAComplaintWithIncorrectEmailByAUser
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
            ConsumerEmail = "wrong@email.com",
            ConsumerPostcode = "S1 4RA",
            Feedback = "This is feedback by the consumer when the user closes the complaint.",
            Reason = "test reason"
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
            Assert.That(closeResponse.ErrorMessage, Is.EqualTo("Provided details do not match the details provided when opening the complaint"));
        });
    }
}
