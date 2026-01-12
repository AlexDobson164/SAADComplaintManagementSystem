namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
internal class GivenARequestToAddANoteToComplaint
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    ViewComplaintResponse complaintWithNotesPrivate;
    ViewComplaintResponse complaintWithNotesPublic;

    Guid userReference = Guid.NewGuid();
    [SetUp]
    public async Task SetUpAsync()
    {
        var businessReference = Guid.NewGuid();
            
        var complaintResponse = await complaintsHostedService.CreateComplaint(new CreateComplaintRequest
        {
            BusinessReference = businessReference,
            FirstMessage = "note",
            ConsumerEmail = "email@email.com",
            ConsumerPostcode = "S1 4RA"
        }, CancellationToken.None);

        var complaintReference = complaintResponse.ComplaintReference.Value;

        // adding a private note
        await complaintsHostedService.AddUserNoteToComplaint(new CreateNewNoteUserRequest
        {
            ComplaintReference = complaintReference,
            BusinessReference = businessReference,
            UserReference = userReference,
            NoteText = "this is a private note by a user.",
            IsPublic = false
        }, CancellationToken.None);
        // adding a public note
        await complaintsHostedService.AddUserNoteToComplaint(new CreateNewNoteUserRequest
        {
            ComplaintReference = complaintReference,
            BusinessReference = businessReference,
            UserReference = userReference,
            NoteText = "this is a public note by a user.",
            IsPublic = true
        }, CancellationToken.None);
        // adding a note by the consumer
        await complaintsHostedService.AddConsumerNoteToComplaint(new CreateNewNoteConsumerRequest
        {
            ComplaintReference = complaintReference,
            BusinessReference = businessReference,
            NoteText = "this is a note by a consumer."
        }, CancellationToken.None);

        complaintWithNotesPrivate = await complaintsHostedService.ViewComplaint(new ViewComplaintRequest
        {
            ComplaintReference = complaintReference,
            BusinessReference = businessReference,
            GetPrivate = true
        }, CancellationToken.None);

        complaintWithNotesPublic = await complaintsHostedService.ViewComplaint(new ViewComplaintRequest
        {
            ComplaintReference = complaintReference,
            BusinessReference = businessReference,
            GetPrivate = false
        }, CancellationToken.None);
    }
    [Test]
    public async Task ThenThereAreTheCorrectNumberOfNotesOnTheTicket()
    {
        Assert.Multiple(() =>
        {
            Assert.That(complaintWithNotesPublic.Notes.Count, Is.EqualTo(3));
            Assert.That(complaintWithNotesPrivate.Notes.Count, Is.EqualTo(4));
        });
    }
    [Test]
    public async Task ThenTheInformationInPublicNotesAreCorrect()
    {
        var userNote = complaintWithNotesPublic.Notes.FirstOrDefault(x => x.UserReference == userReference);
        var consumerNote = complaintWithNotesPublic.Notes.FirstOrDefault(x => x.UserReference == Guid.Parse("87de9d86-4079-4b0a-8368-fd037f0fc38f"));
        Assert.Multiple(() =>
        {
            Assert.That(userNote.NoteText, Is.EqualTo("this is a public note by a user."));
            Assert.That(consumerNote.NoteText, Is.EqualTo("this is a note by a consumer."));
        });
    }
    [Test]
    public async Task ThenTheInformationInPrivateNotesAreCorrect()
    {
        var note = complaintWithNotesPrivate.Notes.FirstOrDefault(x => x.IsPublic == false);
        Assert.That(note.NoteText, Is.EqualTo("this is a private note by a user."));
    }
}