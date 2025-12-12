public class ConsumerCloseComplaintRequest
{
    public Guid ComplaintReference { get; set; }
    public Guid BusinessReference { get; set; }
    public string ConsumerEmail { get; set; }
    public string ConsumerPostcode { get; set; }
    public string Feedback { get; set; }
}
public class ConsumerCloseComplaintResponse
{
    public bool IsSuccessful { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    
}
