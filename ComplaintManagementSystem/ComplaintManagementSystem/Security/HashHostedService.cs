using System.Security.Cryptography;
using System.Text;
public class HashHostedService
{
    public HashPasswordAndGenerateSaltResponse HashPasswordAndGenerateSalt(HashPasswordAndGenerateSaltRequest request)
    {
        // based this salt generation off of the code here - https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-10.0
        string salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        
        return new HashPasswordAndGenerateSaltResponse
        {
            HashsedPassword = HashPassword(new HashPasswordRequest
            {
                Password = request.Password,
                Salt = salt
            }).HashedPassword,
            Salt = salt
        };
    }

    public HashPasswordResponse HashPassword (HashPasswordRequest request)
    {
        string hashedPassword = "";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes((request.Password + request.Salt)));
            foreach (byte b in bytes)
            {
                hashedPassword += b.ToString("x2");
            }
        }
        return new HashPasswordResponse
        {
            HashedPassword = hashedPassword
        };
    }
}