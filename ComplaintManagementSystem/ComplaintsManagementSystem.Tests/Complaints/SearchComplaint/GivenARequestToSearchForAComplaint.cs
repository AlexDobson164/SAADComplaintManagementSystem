namespace ComplaintManagementSystem.Tests.Complaints;
[TestFixture]
public class GivenARequestToSearchForAComplaint
{
    ComplaintsHostedService complaintsHostedService = new ComplaintsHostedService();
    Guid complaintReference;
    [OneTimeSetUp]
    public async Task SetUp()
    {
        var response = await complaintsHostedService.CreateComplaint(new CreateComplaintRequest
        {
            BusinessReference = Guid.Empty,
            FirstMessage = "this is a complaint to search",
            ConsumerEmail = "searchemail@searchemail.com",
            ConsumerPostcode = "ST16 2NS"
        }, CancellationToken.None);

        complaintReference = response.ComplaintReference.Value;
    }

    [Test]
    public async Task ThenTheComplaintCanBeSearchedWithComplaintReference()
    {
        var searchResults = await complaintsHostedService.SearchComplaints(new ComplaintsSearchRequest
        {
            BusinessReference = Guid.Empty,
            ComplaintReference = complaintReference
        }, CancellationToken.None);

        var complaint = searchResults.Complaints[0];

        Assert.Multiple(() =>
        {
            Assert.That(complaint.Reference, Is.EqualTo(complaintReference));
            Assert.That(complaint.FirstMessage, Is.EqualTo("this is a complaint to search"));
        });
    }
    [Test]
    public async Task ThenTheComplaintCanBeSearchedWithPostcode()
    {
        var searchResults = await complaintsHostedService.SearchComplaints(new ComplaintsSearchRequest
        {
            BusinessReference = Guid.Empty,
            ConsumerPostCode = "ST16 2NS"
        }, CancellationToken.None);

        var complaint = searchResults.Complaints[0];

        Assert.Multiple(() =>
        {
            Assert.That(complaint.Reference, Is.EqualTo(complaintReference));
            Assert.That(complaint.FirstMessage, Is.EqualTo("this is a complaint to search"));
        });
    }
    [Test]
    public async Task ThenTheComplaintCanBeSearchedWithEmail()
    {
        var searchResults = await complaintsHostedService.SearchComplaints(new ComplaintsSearchRequest
        {
            BusinessReference = Guid.Empty,
            ConsumerEmail = "searchemail@searchemail.com"
        }, CancellationToken.None);

        var complaint = searchResults.Complaints[0];

        Assert.Multiple(() =>
        {
            Assert.That(complaint.Reference, Is.EqualTo(complaintReference));
            Assert.That(complaint.FirstMessage, Is.EqualTo("this is a complaint to search"));
        });
    }
    [Test]
    public async Task ThenTheComplaintCanBeSearchedWithFirstNote()
    {
        var searchResults = await complaintsHostedService.SearchComplaints(new ComplaintsSearchRequest
        {
            BusinessReference = Guid.Empty,
            FirstNote = "this is a complaint to search"
        }, CancellationToken.None);

        var complaint = searchResults.Complaints[0];

        Assert.Multiple(() =>
        {
            Assert.That(complaint.Reference, Is.EqualTo(complaintReference));
            Assert.That(complaint.FirstMessage, Is.EqualTo("this is a complaint to search"));
        });
    }
}
