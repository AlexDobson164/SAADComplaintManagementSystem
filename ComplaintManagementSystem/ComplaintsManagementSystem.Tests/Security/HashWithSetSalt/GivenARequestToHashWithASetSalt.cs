namespace ComplaintManagementSystem.Tests.Security;
[TestFixture]
public class GivenARequestToHashWithASetSalt
{
    HashHostedService hashHostedService = new HashHostedService();
    HashPasswordResponse firstResponse;
    HashPasswordResponse secondResponse;
    [SetUp]
    public void SetUp()
    {
        string salt = "this is a bad salt";
        firstResponse = hashHostedService.HashPassword(new HashPasswordRequest
        {
            Password = "password",
            Salt = salt
        });
        secondResponse = hashHostedService.HashPassword(new HashPasswordRequest
        {
            Password = "password",
            Salt = salt
        });
    }
    [Test]
    public void ThenThePasswordIsHashedIsHashedCosistently()
    {
        Assert.Multiple(() =>
        {
            Assert.That(firstResponse.HashedPassword, Is.EqualTo(secondResponse.HashedPassword));
            Assert.That(firstResponse.HashedPassword.Length, Is.EqualTo(64));
        });
    }
}
