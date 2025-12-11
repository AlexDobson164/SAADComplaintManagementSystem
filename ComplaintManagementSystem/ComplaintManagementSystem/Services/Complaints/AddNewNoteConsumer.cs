using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

public class AddNewNoteConsumerNoteRequest
{
    [Required]
    public Guid ComplaintReference { get; set; }
    [Required]
    public string NoteText { get; set; }
}
public class AddNewNoteConsumerResponse
{
    public bool IsSuccessful { get; set; }
    public List<string> Errors { get; set; }
}