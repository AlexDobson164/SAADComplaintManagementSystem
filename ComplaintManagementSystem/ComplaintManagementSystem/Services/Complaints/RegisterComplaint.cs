using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

public class RegisterComplaintRequest
{
    [Required]
    public string ConsumerEmail { get; set; }
    [Required]
    public string ConsumerPostCode { get; set; }
    [Required]
    public string NoteText { get; set; }
}
public class RegisterComplaintResponse
{
    public Guid? ComplaintReference { get; set; }
    public bool IsSuccessful { get; set; }
    public List<string> Errors { get; set; }
}