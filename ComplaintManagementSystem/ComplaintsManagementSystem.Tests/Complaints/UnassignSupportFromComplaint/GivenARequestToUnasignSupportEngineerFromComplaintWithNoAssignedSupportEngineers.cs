using Microsoft.AspNetCore.Http;
using NuGet.Frameworks;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToUnasignSupportEngineerFromComplaintWithNoAssignedSupportEngineers
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    Guid userReference = Guid.NewGuid();
    UnassignSupportEngineerFromComplaintResponse unassignResponse;
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

        unassignResponse = await complaintsHostedService.UnassignSupportEngineer(new UnassignSupportEngineerFromComplaintRequest
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
            Assert.That(unassignResponse.IsSuccessful, Is.False);
            Assert.That(unassignResponse.ErrorCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(unassignResponse.Error, Is.EqualTo("User is not assigned to complaint"));
        });
    }
}