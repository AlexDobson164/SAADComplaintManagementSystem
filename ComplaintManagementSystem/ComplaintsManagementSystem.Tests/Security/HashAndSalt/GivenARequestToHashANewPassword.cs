namespace ComplaintManagementSystem.Tests.Security;
[TestFixture]
public class GivenARequestToHashANewPassword
{
    HashHostedService hashHostedService = new HashHostedService();
    HashPasswordAndGenerateSaltResponse firstResponse;
    HashPasswordAndGenerateSaltResponse secondResponse;
    [SetUp]
    public void SetUp()
    {
        firstResponse = hashHostedService.HashPasswordAndGenerateSalt(new HashPasswordAndGenerateSaltRequest
        {
            Password = "password"
        });
        secondResponse = hashHostedService.HashPasswordAndGenerateSalt(new HashPasswordAndGenerateSaltRequest
        {
            Password = "password"
        });
    }
    [Test]
    public void ThenThePasswordIsHashedAndHasAUniqueSalt()
    {
        Assert.Multiple(() =>
        {
            Assert.That(firstResponse.HashsedPassword, Is.Not.EqualTo(secondResponse.HashsedPassword));
            Assert.That(firstResponse.Salt, Is.Not.EqualTo(secondResponse.Salt));
            Assert.That(firstResponse.HashsedPassword.Length, Is.EqualTo(64));
            Assert.That(firstResponse.Salt.Length, Is.EqualTo(24));
        });
    }
}
