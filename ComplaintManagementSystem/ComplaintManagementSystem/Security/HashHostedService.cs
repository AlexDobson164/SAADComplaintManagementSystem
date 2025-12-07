using System.Security.Cryptography;
using System.Text;
public class HashHostedService
{
    public HashPasswordAndGenerateSaltResponse HashPasswordAndGenerateSalt(HashPasswordAndGenerateSaltRequest request)
    {
        // based this salt generation off of the code here - https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-10.0
        //it always generates "==" at the end of the salt so i need to remove it because the convert on line 10 doesnt like it...
        string salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16)).Replace("==", "");
        //split this out later
        string hashedPassword = "";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes((request.Password + salt)));
            foreach (byte b in bytes)
            {
                hashedPassword += b.ToString("x2");
            }
        }
        //byte[] saltedPassword = Convert.FromBase64String(request.Password + salt);
        //string hashedPassword = Convert.ToBase64String(SHA256.HashData(saltedPassword));
        return new HashPasswordAndGenerateSaltResponse
        {
            HashsedPassword = hashedPassword,
            Salt = salt
        };
    }
}