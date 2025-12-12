public class ViewComplaintRequest
{
    public Guid ComplaintReference { get; set; }
    public Guid BusinessReference { get; set; }
    public bool GetPrivate { get; set; }
}
public class ViewComplaintResponse
{
    public List<Note> Notes { get; set; }
    public bool IsOpen { get; set; }
    public bool IsSuccessful { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}
public class Note
{
    public int Id { get; set; }
    public string NoteText { get; set; }
    public Guid UserReference { get; set; }
    public bool IsPublic { get; set; }
    public DateTime TimePosted { get; set; }
}