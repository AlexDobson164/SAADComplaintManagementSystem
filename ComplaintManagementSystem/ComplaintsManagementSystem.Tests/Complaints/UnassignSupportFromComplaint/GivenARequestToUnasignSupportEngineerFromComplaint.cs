using NuGet.Frameworks;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToUnasignSupportEngineerFromComplaint
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    Guid userReference = Guid.NewGuid();
    GetAllComplaintsForUserResponse assignedComplaints;
    Guid complaintReference;
    [SetUp]
    public async Task SetUp()
    {
        var complaintResponse = await complaintsHostedService.CreateComplaint(new CreateComplaintRequest
        {
            BusinessReference = Guid.Empty,
            FirstMessage = "note",
            ConsumerEmail = "email@email.com",
            ConsumerPostcode = "S1 4RA"
        }, CancellationToken.None);

        complaintReference = complaintResponse.ComplaintReference.Value;

        var assignResponse = await complaintsHostedService.AssignSupportEngineer(new AssignSupportEngineerToComplaintRequest
        {
            BusinessReference = Guid.Empty,
            UserReference = userReference,
            ComplaintReference = complaintReference
        }, CancellationToken.None);

        var unassighResponse = await complaintsHostedService.UnassignSupportEngineer(new UnassignSupportEngineerFromComplaintRequest
        {
            BusinessReference = Guid.Empty,
            UserReference = userReference,
            ComplaintReference = complaintReference
        }, CancellationToken.None);

        assignedComplaints = await complaintsHostedService.GetAllComplaintsForUser(new GetAllComplaintsForUserRequest
        {
            UserReference = userReference,
            BusinessReference = Guid.Empty
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheUserIsAssignedToTheComplaint()
    {
        Assert.That(assignedComplaints.Complaints.Count, Is.EqualTo(0));
    }
}