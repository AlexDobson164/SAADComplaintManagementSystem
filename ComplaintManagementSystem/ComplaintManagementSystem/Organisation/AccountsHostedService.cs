public class AccountsHostedService
{
    public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        List<string> errorMessages = new List<string>();
        if (!Validation.ValidateEmail(request.Email))
            errorMessages.Add("Invalid Email");
        //make sure the email is not already used
        if (UserTable.IsEmailInUse(request.Email, cancellationToken).Result)
            errorMessages.Add("Email is already in use");
        //make sure the business ref is valid
        if (!BusinessTable.BusinessExistsByRef(request.BusinessReferece, cancellationToken).Result)
            errorMessages.Add("Business does not exist");

        if (errorMessages.Count != 0)
            return new CreateAccountResponse
            {
                IsSuccessful = false,
                ErrorMessages = errorMessages.ToArray()
            };

        await UserTable.SaveNewUser(new User
        {
            Reference = Guid.NewGuid(),
            Email = request.Email,
            BusinessReference = request.BusinessReferece,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role
        }, request.HashedPassword,
        request.Salt, cancellationToken);

        return new CreateAccountResponse
        {
            IsSuccessful = true,
        };
    }

    public async Task<AuthInfoResponse> GetAuthInfoByEmail(AuthInfoRequest request, CancellationToken cancellationToken)
    { 
        var record = await UserTable.GetPasswordAndSalt(request.Email, cancellationToken);
        if (record == null)
            return new AuthInfoResponse();

        return new AuthInfoResponse
        {
            HashedPassword = record.Password,
            Salt = record.Salt
        };
    }

    public async Task<GetUserByEmailResponse> GetUserByEmail(GetUserByEmailRequest request, CancellationToken cancellationToken)
    {
        var record = await UserTable.GetUserByEmail(request.Email, cancellationToken);
        return new GetUserByEmailResponse
        {
            Reference = record.Reference,
            Email = record.Email,
            BusinessReference = record.BusinessReference,
            FirstName = record.FirstName,
            LastName = record.LastName,
            Role = record.Role
        };
    }
    public async Task<GetNameByUserReferenceResponse> GetNameByUserRef(GetNameByUserReferenceRequest request, CancellationToken cancellationToken)
    {
        var response = await UserTable.GetNameByReference(request.UserReference, cancellationToken);
        if (String.IsNullOrEmpty(response))
            return new GetNameByUserReferenceResponse
            {
                IsSuccessful = false,
                Error = "User not found"
            };
        return new GetNameByUserReferenceResponse
        {
            Name = response,
            IsSuccessful = true
        };
    }
}
