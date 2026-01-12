using NHibernate.Mapping.ByCode.Impl;
using System.Linq;

namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToAddANoteToAComplaintThatDoesntExist
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    CreateNewNoteConsumerResponse consumerResponse;
    CreateNewNoteUserResponse userResponse;
    [SetUp]
    public async Task SetUp()
    {
        consumerResponse = await complaintsHostedService.AddConsumerNoteToComplaint(new CreateNewNoteConsumerRequest
        {
            ComplaintReference = Guid.NewGuid(),
            BusinessReference = Guid.Empty,
            NoteText = ""
        }, CancellationToken.None);

        userResponse = await complaintsHostedService.AddUserNoteToComplaint(new CreateNewNoteUserRequest
        {
            ComplaintReference = Guid.NewGuid(),
            BusinessReference = Guid.Empty,
            UserReference = Guid.Empty,
            NoteText = "",
            IsPublic = true,
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenTheCorrectErrorIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(consumerResponse.IsSuccess, Is.False);
            Assert.That(consumerResponse.Errors.Contains("requested complaint does not exist"), Is.True);
            Assert.That(userResponse.IsSuccess, Is.False);
            Assert.That(userResponse.Errors.Contains("requested complaint does not exist"), Is.True);
        });
    }
}

