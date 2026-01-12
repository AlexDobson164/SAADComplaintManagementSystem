using NUnit.Framework.Internal;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenThatARequestToCreateAComplaintWithCorrectDetails
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    CreateComplaintResponse response;

    [SetUp]
    public async Task SetUpAsync()
    {
        var request = new CreateComplaintRequest
        {
            BusinessReference = Guid.Empty,
            FirstMessage = "test note",
            ConsumerEmail = "email@email.com",
            ConsumerPostcode = "S1 4RA"
        };
        response = await complaintsHostedService.CreateComplaint(request, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheComplaintCanBeSearchedWithTheComplaintReference()
    {
        var complaint = await complaintsHostedService.SearchComplaints(new ComplaintsSearchRequest
        {
            ComplaintReference = response.ComplaintReference
        }, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(complaint.IsSuccess, Is.True);
            Assert.That(complaint.Complaints[0].FirstMessage, Is.EqualTo("test note"));
        });
    }
}