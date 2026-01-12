using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToViewAComplaintThatDoesntExist
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    ViewComplaintResponse response;
    [SetUp]
    public async Task Setup()
    {
        response = await complaintsHostedService.ViewComplaint(new ViewComplaintRequest
        {
            ComplaintReference = Guid.NewGuid(),
            BusinessReference = Guid.Empty,
            GetPrivate = true
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectErrorIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessful, Is.False);
            Assert.That(response.ErrorCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(response.ErrorMessage, Is.EqualTo("Complaint does not exist"));
        });
    }
}