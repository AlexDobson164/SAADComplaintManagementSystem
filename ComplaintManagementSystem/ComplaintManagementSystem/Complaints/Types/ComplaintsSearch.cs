using NHibernate.Mapping.ByCode.Impl;

public class ComplaintsSearchRequest
{
    public string? FirstNote { get; set; }
    public string? ConsumerEmail { get; set; }
    public string? ConsumerPostCode { get; set; }
    public Guid? ComplaintReference { get; set; }
    public bool? IsOpen { get; set; }
    public Guid BusinessReference { get; set; }
}
public class ComplaintsSearchResponse
{
    public List<ComplaintInformation> Complaints { get; set; } = new();
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; }
}
public class ComplaintInformation
{
    public Guid Reference { get; set; }
    public string FirstMessage { get; set; }
    public DateTime TimeOpened { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsOpen { get; set; }
}