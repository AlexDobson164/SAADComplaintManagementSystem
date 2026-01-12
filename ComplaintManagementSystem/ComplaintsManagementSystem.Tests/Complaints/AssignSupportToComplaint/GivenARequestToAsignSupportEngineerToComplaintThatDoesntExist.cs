using Microsoft.AspNetCore.Http;
using NuGet.Frameworks;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToAsignSupportEngineerToComplaintThatDoesntExist
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    AssignSupportEngineerToComplaintResponse response;
    [SetUp]
    public async Task SetUp()
    {
        response = await complaintsHostedService.AssignSupportEngineer(new AssignSupportEngineerToComplaintRequest
        {
            BusinessReference = Guid.Empty,
            UserReference = Guid.Empty,
            ComplaintReference = Guid.NewGuid()
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheUserIsAssignedToTheComplaint()
    {
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessful, Is.False);
            Assert.That(response.ErrorCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(response.Error, Is.EqualTo("Complaint Not Found"));
        });
    }
}