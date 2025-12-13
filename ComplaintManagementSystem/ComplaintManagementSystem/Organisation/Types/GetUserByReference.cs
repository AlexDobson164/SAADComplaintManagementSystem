public class GetUserByReferenceRequest
{
    public Guid UserReference { get; set; }
    public Guid BusinessReference { get; set; }
}
public class GetUserByReferenceResponse
{
    public User User { get; set; }
    public bool IsSuccessful { get; set; }
    public string Error { get; set; }
}