using System.ComponentModel.DataAnnotations;

public class AddNewNoteUserRequest
{
    [Required]
    public Guid ComplaintReference { get; set; }
    [Required]
    public string NoteText { get; set; }
    [Required]
    public bool IsPublic { get; set; }
}
public class AddNewNoteUserResponse
{
    public bool IsSuccessful { get; set; }
    public List<string> Errors { get; set; }
}