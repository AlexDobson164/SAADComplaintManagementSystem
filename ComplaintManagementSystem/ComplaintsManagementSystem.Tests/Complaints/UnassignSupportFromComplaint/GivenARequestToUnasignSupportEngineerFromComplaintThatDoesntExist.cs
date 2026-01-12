using Microsoft.AspNetCore.Http;
using NuGet.Frameworks;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToUnasignSupportEngineerFromComplaintThatDoesntExist
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    Guid userReference = Guid.NewGuid();
    UnassignSupportEngineerFromComplaintResponse unassignResponse;
    Guid complaintReference;
    [SetUp]
    public async Task SetUp()
    {
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
            Assert.That(unassignResponse.ErrorCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(unassignResponse.Error, Is.EqualTo("Complaint Not Found"));
        });
    }
}