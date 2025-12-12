using System.ComponentModel.DataAnnotations;

public class CloseComplaintConsumerRequest
{
    [Required]
    public Guid ComplaintReference { get; set; }
    [Required]
    public string ConsumerEmail { get; set; }
    [Required]
    public string ConsumerPostcode { get; set; }
    public string Feedback { get; set; }
}
public class CloseComplaintConsumerResponse
{
    public bool IsSuccessful { get; set; }
    public List<string> Errors { get; set; }
}
