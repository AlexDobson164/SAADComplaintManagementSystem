using System.Reflection.Metadata;

public class GetAllComplaintsForUserRequest
{
    public Guid UserReference { get; set; }
    public Guid BusinessReference { get; set; }
}
public class GetAllComplaintsForUserResponse
{
    public List<ComplaintsInfo> Complaints { get; set; }
    public bool IsSucessful { get; set; }
    public string Error {  get; set; }
}
public class ComplatintInfo
{
    public Guid Reference { get; set; }
    public string FirstMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdated { get; set; }
}