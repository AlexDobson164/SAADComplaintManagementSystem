using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AccountManagementController : ControllerBase
{
    AccountsHostedService AccountsHostedService = new AccountsHostedService();
    HashHostedService HashHostedService = new HashHostedService();

    [HttpPost("AdminCreateAccount", Name = "AdminCreateAccount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CreateAccountResponse> CreateAccount(AdminCreateAccountRequest request)
    {
        //auth checks here later
        var hashInfo = HashHostedService.HashPasswordAndGenerateSalt(new HashPasswordAndGenerateSaltRequest
        {
            Password = request.password
        });
        var response = AccountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = request.emailAddress,
            HashedPassword = hashInfo.HashsedPassword,
            Salt = hashInfo.Salt,
            BusinessReferece = request.businessReference,
            FirstName = request.firstName,
            LastName = request.lastName,
            Role = request.role
        });
        if (response.IsSuccessful)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
