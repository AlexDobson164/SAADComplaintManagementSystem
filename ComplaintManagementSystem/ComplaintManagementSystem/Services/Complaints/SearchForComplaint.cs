public class SearchForComplaintRequest
{
    public string? FirstNote { get; set; }
    public string? ConsumerEmail { get; set; }
    public string? ConsumerPostCode { get; set; }
    public Guid? ComplaintReference { get; set; }
    public bool? Is_Open { get; set; }
}
public class SearchForComplaintResponse
{
    public List<Complaint> Complaints { get; set; } = new();
    public bool IsSuccess { get; set; } = true;
    public List<string> Errors { get; set; } = new();
}

public class Complaint
{
    public Guid Reference { get; set; }
    public string FirstMessage { get; set; }
    public DateTime TimeOpened { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsOpen { get; set; }
}