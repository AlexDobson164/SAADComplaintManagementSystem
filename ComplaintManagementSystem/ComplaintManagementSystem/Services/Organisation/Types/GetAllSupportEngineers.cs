public class GetAllSupportEngineersResponse
{
    public List<SupportEngineer> SupportEngineers { get; set; }
    public bool IsSuccessful { get; set; }
}
public class SupportEngineer
{
    public Guid UserReference { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}