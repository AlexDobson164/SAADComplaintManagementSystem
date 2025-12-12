public class ViewComplaintUserRequest
{
    public Guid ComplaintReference { get; set; }
}
public class ViewComplaintUserResponse
{
    public List<ComplaintNoteForUser> Notes { get; set; }
    public bool IsOpen { get; set; }
    public bool IsSucessful { get; set; }
    public List<string> Errors { get; set; }
}
public class ComplaintNoteForUser
{
    public string NoteText { get; set; }
    public string SenderName { get; set; }
    public Guid SenderReference { get; set; }
    public DateTime TimeNoteSent { get; set; }
    public bool IsPublic { get; set; }
}