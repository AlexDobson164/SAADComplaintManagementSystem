public class AuthInfoRequest
{
    public string Email { get; set; }
}
public class AuthInfoResponse
{
    public string HashedPassword { get; set; }
    public string Salt { get; set; }
}
