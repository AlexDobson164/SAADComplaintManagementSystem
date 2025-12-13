public class UnassignSupportEngineerFromComplaintRequest
{
    public Guid UserReference { get; set; }
    public Guid BusinessReference { get; set; }
    public Guid ComplaintReference { get; set; }
}
public class UnassignSupportEngineerFromComplaintResponse
{
    public bool IsSuccessful { get; set; }
    public int ErrorCode { get; set; }
    public string Error { get; set; }
}
