public class CreateNewNoteUserRequest
{
    public Guid ComplaintReference { get; set; }
    public Guid BusinessReference { get; set; }
    public Guid UserReference { get; set; }
    public string NoteText { get; set; }
    public bool IsPublic { get; set; }
}
public class CreateNewNoteUserResponse
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}
