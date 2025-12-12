using System.ComponentModel.DataAnnotations;

public class CloseComplaintUserRequest
{
    [Required]
    public Guid ComplaintReference { get; set; }

    public string? ConsumerEmail { get; set; }
    public string? ConsumerPostcode { get; set; }
    public string? Reason { get; set; }
    public string? Feedback { get; set; }
}
public class CloseComplaintUserResponse
{
    public bool IsSuccessful { get; set; }
    public List<string> Errors { get; set; }
}
