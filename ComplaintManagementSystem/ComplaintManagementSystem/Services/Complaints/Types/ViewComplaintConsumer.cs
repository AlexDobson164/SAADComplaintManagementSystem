public class ViewComplaintConsumerRequest
{
    public Guid ComplaintReference { get; set; }
}
public class ViewComplaintConsumerResponse
{
    public List<ComplaintNoteForConsumer> Notes { get; set; }
    public bool IsOpen { get; set; }
    public bool IsSucessful { get; set; }
    public List<string> Errors { get; set; }
}
public class ComplaintNoteForConsumer
{
    public string NoteText { get; set; }
    public string SenderName { get; set; }
    public Guid SenderReference { get; set; }
    public DateTime TimeNoteSent { get; set; }
}