using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ComplaintsController : ControllerBase
{
    ComplaintsHostedService ComplaintsHostedService = new ComplaintsHostedService();
    EmailHostedService EmailHostedService = new EmailHostedService();
    BusinessHostedService BusinessHostedService = new BusinessHostedService();

    [HttpPost("RegisterComplaint", Name = "RegisterComplaint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RegisterComplaintResponse> RegisterComplaint(RegisterComplaintRequest request)
    {
        // create a complaint record
        // create a note
        //send email with recored ref

        HttpContext.Request.Headers.TryGetValue("x-api-key", out var apiKey);

        var userBusinessReference = BusinessHostedService.ValidateApiKey(new ValidateApiKeyRequest
        {
            ApiKey = Guid.Parse(apiKey) // this can not be null as it would not got through the middleware if there was no api key
        }).BusinessReference;

        var response = ComplaintsHostedService.CreateComplaint(new CreateComplaintRequest
        {
            ConsumerEmail = request.ConsumerEmail.ToLower(),
            ConsumerPostcode = request.ConsumerPostCode.ToUpper(),
            FirstMessage = request.NoteText,
            BusinessReference = userBusinessReference
        });

        if (!response.IsSuccessful)
            return BadRequest(new RegisterComplaintResponse
            {
                IsSuccessful = false,
                Errors = response.Errors
            });

        EmailHostedService.SendComplaintEmail(new SendComplaintEmailRequest
        {
            ReceivingEmail = request.ConsumerEmail,
            SendingEmail = "Complaints@ComplaintsManagementSystem.com",
            EmailText = $"""
            Hi,
            Thank you for registering your complaint, we hope to get to you shortly

            To veiw your complaint, click here
            {response.ComplaintReference}

            Thank you
            Complaint Management System
            """
        });
        return new RegisterComplaintResponse
        {
            ComplaintReference = response.ComplaintReference,
            IsSuccessful = response.IsSuccessful,
            Errors = response.Errors
        };
    }
}
