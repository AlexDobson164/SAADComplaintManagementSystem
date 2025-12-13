public class GetAllComplaintsForUser
{
    public List<ComplaintsInfo> Complaints { get; set; }
    public bool IsSuccessful { get; set; }
    public List<string> Errors { get; set; }
}
public class ComplaintsInfo
{
    public Guid Reference { get; set; }
    public string FirstMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdated { get; set; }
}