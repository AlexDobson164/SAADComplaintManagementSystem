public class GetBusinessReferenceFromHttpContextRequest
{
    public HttpContext Context { get; set; }
}
public class GetBusinessReferenceFromHttpContextResponse
{
    public Guid BusinessReference { get; set; }
}