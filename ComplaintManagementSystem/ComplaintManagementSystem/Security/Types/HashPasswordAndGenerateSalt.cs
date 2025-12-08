public class HashPasswordAndGenerateSaltRequest
{
    public string Password { get; set; }
}
public class HashPasswordAndGenerateSaltResponse
{
    public string HashsedPassword { get; set; }
    public string Salt { get; set; }
}