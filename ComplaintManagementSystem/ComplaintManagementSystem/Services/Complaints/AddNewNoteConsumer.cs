using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

public class AddNewConsumerNoteRequest
{
    [Required]
    public Guid ComplaintReference { get; set; }
    [Required]
    public string NoteText { get; set; }
}
public class AddNewNoteConsumerResponse
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; }
}