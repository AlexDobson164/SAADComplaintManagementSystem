using System.Xml.Linq;

public class AccountsHostedService
{
    public CreateAccountResponse CreateAccount(CreateAccountRequest request)
    {
        List<string> errorMessages = new List<string>();
        if (!Validation.ValidateEmail(request.Email))
            errorMessages.Add("Invalid Email");
        //make sure the email is not already used
        if (UserTable.IsEmailInUse(request.Email))
            errorMessages.Add("Email is already in use");
        //make sure the business ref is valid
        if (!BusinessTable.BusinessExistsByRef(request.BusinessReferece))
            errorMessages.Add("Business does not exist");

        if (errorMessages.Count != 0)
            return new CreateAccountResponse
            {
                IsSuccessful = false,
                ErrorMessages = errorMessages.ToArray()
            };

        UserTable.SaveNewUser(new User
        {
            Reference = Guid.NewGuid(),
            Email = request.Email,
            BusinessReference = request.BusinessReferece,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role
        }, request.HashedPassword,
        request.Salt);

        return new CreateAccountResponse
        {
            IsSuccessful = true,
        };
    }
}
