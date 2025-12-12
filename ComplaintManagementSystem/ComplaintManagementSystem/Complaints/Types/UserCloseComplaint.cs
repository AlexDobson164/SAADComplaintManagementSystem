public class UserCloseComplaintRequest
{
    public Guid ComplaintReference { get; set; }
    public Guid UserReference { get; set; }
    public Guid BusinessReference { get; set; }
    public string ConsumerEmail { get; set; }
    public string ConsumerPostcode { get; set; }
    public string Reason { get; set; }
    public string Feedback { get; set; }
}
public class UserCloseComplaintResponse
{
    public string ConsumerEmail { get; set; }

    public bool IsSuccessful { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}
