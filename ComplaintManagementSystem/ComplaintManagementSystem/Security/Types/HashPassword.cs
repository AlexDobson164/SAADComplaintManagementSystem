public class HashPasswordRequest
{
    public string Password { get; set; }
    public string Salt { get; set; }
}

public class HashPasswordResponse
{
    public string HashedPassword { get; set; }
}