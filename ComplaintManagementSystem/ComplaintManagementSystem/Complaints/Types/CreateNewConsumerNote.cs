using Org.BouncyCastle.Asn1.Mozilla;

public class CreateNewNoteConsumerRequest
{
    public Guid ComplaintReference { get; set; }
    public Guid BusinessReference { get; set; } 
    public string NoteText { get; set; }
}
public class CreateNewNoteConsumerResponse
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}
