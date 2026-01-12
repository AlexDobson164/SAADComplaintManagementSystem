using Microsoft.AspNetCore.Http;
using NuGet.Frameworks;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToAsignSupportEngineerToComplaintThatTheyAreAlreadyAssignedTo
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    AssignSupportEngineerToComplaintResponse secondAssignmentResponse;
    [SetUp]
    public async Task SetUp()
    {
        Guid userReference = Guid.NewGuid();

        var complaintResponse = await complaintsHostedService.CreateComplaint(new CreateComplaintRequest
        {
            BusinessReference = Guid.Empty,
            FirstMessage = "note",
            ConsumerEmail = "email@email.com",
            ConsumerPostcode = "S1 4RA"
        }, CancellationToken.None);

        var complaintReference = complaintResponse.ComplaintReference.Value;

        await complaintsHostedService.AssignSupportEngineer(new AssignSupportEngineerToComplaintRequest
        {
            BusinessReference = Guid.Empty,
            UserReference = userReference,
            ComplaintReference = complaintReference
        }, CancellationToken.None);

        secondAssignmentResponse = await complaintsHostedService.AssignSupportEngineer(new AssignSupportEngineerToComplaintRequest
        {
            BusinessReference = Guid.Empty,
            UserReference = userReference,
            ComplaintReference = complaintReference
        }, CancellationToken.None);

    }
    [Test]
    public async Task ThenTheCorrectErrorIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(secondAssignmentResponse.IsSuccessful, Is.False);
            Assert.That(secondAssignmentResponse.ErrorCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(secondAssignmentResponse.Error, Is.EqualTo("Engineer is already assigned to complaint"));
        });
    }
}