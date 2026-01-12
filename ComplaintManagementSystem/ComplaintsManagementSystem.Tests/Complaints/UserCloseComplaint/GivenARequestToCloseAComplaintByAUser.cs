using System.Linq;
using System.Runtime.InteropServices;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToCloseAComplaintByAUser
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
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

        var closeResponse = await complaintsHostedService.UserCloseComplaint(new UserCloseComplaintRequest
        {
            BusinessReference = Guid.Empty,
            ComplaintReference = complaintReference,
            ConsumerEmail = "email@email.com",
            ConsumerPostcode = "S1 4RA",
            Feedback = "This is feedback by the consumer when the user closes the complaint.",
            Reason = "this is a test reason"
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheComplaintIsClosedAndHasTheCorrectFeedback()
    {
        var complaintInfo = await complaintsHostedService.ViewComplaint(new ViewComplaintRequest
        {
            ComplaintReference = complaintReference,
            BusinessReference = Guid.Empty,
            GetPrivate = true
        }, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(complaintInfo.IsOpen, Is.False);
            Assert.That(complaintInfo.Notes.FirstOrDefault(x => x.IsPublic == false).NoteText, Is.EqualTo("This is feedback by the consumer when the user closes the complaint."));
        });
    }
}
