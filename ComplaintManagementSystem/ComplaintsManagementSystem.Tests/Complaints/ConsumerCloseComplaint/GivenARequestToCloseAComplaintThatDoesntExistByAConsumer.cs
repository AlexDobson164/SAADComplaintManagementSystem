using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Runtime.InteropServices;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToCloseAComplaintThatDoesntExistByAConsumer
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    ConsumerCloseComplaintResponse closeResponse;
    [SetUp]
    public async Task Setup()
    {
        
        closeResponse = await complaintsHostedService.ConsumerCloseComplaint(new ConsumerCloseComplaintRequest
        {
            BusinessReference = Guid.Empty,
            ComplaintReference = Guid.NewGuid(),
            ConsumerEmail = "email@email.com",
            ConsumerPostcode = "S1 4RA",
            Feedback = "This is feedback by the consumer when they close the complaint."
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectErrorIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(closeResponse.IsSuccessful, Is.False);
            Assert.That(closeResponse.ErrorCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(closeResponse.ErrorMessage, Is.EqualTo("Complaint does not exist"));
        });
    }
}
