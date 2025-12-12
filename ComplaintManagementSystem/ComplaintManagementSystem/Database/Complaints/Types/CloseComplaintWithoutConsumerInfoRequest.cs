public class CloseComplaintWithoutConsumerInfoRequest
{ 
    public Guid ComplaintReference { get; set; }
    public Guid BusinessReference { get; set; }
    public Guid UserReference { get; set; }
    public string CloseReason { get; set; }
}
public class CloseComplaintWithoutConsumerInfoResponse
{
    public bool IsSuccessful { get; set; }
    public string ConsumerEmail { get; set; }
}