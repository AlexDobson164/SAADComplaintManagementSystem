public class CreateComplaintRequest
{
    public string ConsumerEmail { get; set; }
    public string ConsumerPostcode { get; set; }
    public string FirstMessage { get; set; }
    public Guid BusinessReference { get; set; }
}
public class CreateComplaintResponse
{
    public Guid? ComplaintReference { get; set; }
    public bool IsSuccessful { get; set; } 
    public List<string> Errors { get; set; }
}