public class AssignSupportEngineerRequest
{
    public Guid UserReference { get; set; }
    public Guid ComplaintReference { get; set; }
}
public class AssignSupportEngineerResponse
{
    public bool IsSuccessful { get; set; }
    public List<string> Errors { get; set; }
}