using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    AccountsHostedService AccountsHostedService = new AccountsHostedService();
    HashHostedService HashHostedService = new HashHostedService();

    [HttpPost("AdminCreateAccount", Name = "AdminCreateAccount"), BasicAuth]
    [ProducesResponseType(typeof(CreateAccountResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> CreateAccount(AdminCreateAccountRequest request, CancellationToken cancellationToken)
    {
        var user = new AuthedUser(User);

        if (user.Role != RolesEnum.SystemAdmin)
            return Results.Unauthorized();

        var hashInfo = HashHostedService.HashPasswordAndGenerateSalt(new HashPasswordAndGenerateSaltRequest
        {
            Password = request.password
        });
        var response = await AccountsHostedService.CreateAccount(new CreateAccountRequest
        {
            Email = request.emailAddress,
            HashedPassword = hashInfo.HashsedPassword,
            Salt = hashInfo.Salt,
            BusinessReferece = request.businessReference,
            FirstName = request.firstName,
            LastName = request.lastName,
            Role = request.role
        }, cancellationToken);
        if (response.IsSuccessful)
        {
            return Results.Ok(response);
        }
        return Results.BadRequest(response);
    }

    [HttpGet("GetAllSupportEngineers", Name = "GetAllSupportEngineers"), BasicAuth]
    [ProducesResponseType(typeof(GetAllSupportEngineersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> GetAllSupportEngineers(CancellationToken cancellationToken)
    {
        var user = new AuthedUser(User);

        if (user.Role == RolesEnum.Consumer)
            return Results.Unauthorized();

        var response = AccountsHostedService.GetAllUsersByRole(new GetAllUsersByRoleRequest
        {
            Role = RolesEnum.SupportEngineer,
            BusinessReference = user.BusinessReference
        },cancellationToken);

        return Results.Ok(new GetAllSupportEngineersResponse
        {
            IsSuccessful = true,
            SupportEngineers = response.Result.Users.ConvertAll(x => new SupportEngineer
            {
                UserReference = x.UserReference,
                Name = x.Name,
                Email = x.Email,
            })
        });
    }
}
