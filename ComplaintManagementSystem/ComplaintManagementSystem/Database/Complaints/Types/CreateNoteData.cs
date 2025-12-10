public class CreateNoteData
{
    public Guid ComplaintReference { get; set; }
    public string NoteText { get; set; }
    public Guid UserReference { get; set; }
    public bool IsPublic { get; set; }
}