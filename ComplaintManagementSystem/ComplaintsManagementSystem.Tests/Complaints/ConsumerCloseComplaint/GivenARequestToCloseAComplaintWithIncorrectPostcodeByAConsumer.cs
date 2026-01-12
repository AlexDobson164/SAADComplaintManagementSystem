using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Runtime.InteropServices;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToCloseAComplaintWithIncorrectPostcodeByAConsumer
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    Guid complaintReference;
    ConsumerCloseComplaintResponse closeResponse;
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

        closeResponse = await complaintsHostedService.ConsumerCloseComplaint(new ConsumerCloseComplaintRequest
        {
            BusinessReference = Guid.Empty,
            ComplaintReference = complaintReference,
            ConsumerEmail = "email@email.com",
            ConsumerPostcode = "ST16 2NS",
            Feedback = "This is feedback by the consumer when they close the complaint."
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheComplaintIsntClosedAndTheCorrectErrorIsReturned()
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
            Assert.That(closeResponse.ErrorCode, Is.EqualTo(StatusCodes.Status401Unauthorized));
            Assert.That(closeResponse.ErrorMessage, Is.EqualTo("Provided details do not match the details provided when opening the complaint"));
        });
    }
}
