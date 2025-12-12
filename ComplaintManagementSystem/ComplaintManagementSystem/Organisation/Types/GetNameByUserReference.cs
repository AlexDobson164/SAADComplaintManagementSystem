public class GetNameByUserReferenceRequest
{
    public Guid UserReference { get; set; }
}

public class GetNameByUserReferenceResponse
{
    public string Name { get; set; }
    public bool IsSuccessful { get; set; }
    public string Error { get; set; }
}