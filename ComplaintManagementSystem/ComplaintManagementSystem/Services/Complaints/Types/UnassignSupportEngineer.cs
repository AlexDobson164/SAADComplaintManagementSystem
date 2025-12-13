public class UnassignSupportEngineerRequest
{
    public Guid UserReference { get; set; }
    public Guid ComplaintReference { get; set; }
}
public class UnassignSupportEngineerResponse
{
    public bool IsSuccessful { get; set; }
    public List<string> Errors { get; set; }
}
