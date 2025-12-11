using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq.Expressions;

[ApiController]
[Route("[controller]")]
public class ComplaintsController : ControllerBase
{
    ComplaintsHostedService ComplaintsHostedService = new ComplaintsHostedService();
    EmailHostedService EmailHostedService = new EmailHostedService();
    BusinessHostedService BusinessHostedService = new BusinessHostedService();

    [HttpPost("RegisterComplaint", Name = "RegisterComplaint")]
    [ProducesResponseType(typeof(RegisterComplaintResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> RegisterComplaint(RegisterComplaintRequest request, CancellationToken cancellationToken)
    {
        var businessReference = await BusinessHostedService.GetBusinessReferenceFromHttpContext(new GetBusinessReferenceFromHttpContextRequest
        {
            Context = HttpContext
        }, cancellationToken);

        var response = await ComplaintsHostedService.CreateComplaint(new CreateComplaintRequest
        {
            ConsumerEmail = request.ConsumerEmail.ToLower(),
            ConsumerPostcode = request.ConsumerPostCode.ToUpper(),
            FirstMessage = request.NoteText,
            BusinessReference = businessReference.BusinessReference
        },cancellationToken).ConfigureAwait(false);
         
        if (!response.IsSuccessful)
            return Results.BadRequest(new RegisterComplaintResponse
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

        return Results.Ok(new RegisterComplaintResponse
        {
            ComplaintReference = response.ComplaintReference,
            IsSuccessful = response.IsSuccessful,
            Errors = response.Errors
        });
    }
    // for these 2 methods, check if the complaint has the correct business ref
    // needs testing
    [HttpPost("AddNoteConsumer", Name = "AddNoteConsumer")]
    [ProducesResponseType(typeof(AddNewNoteConsumerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddNewConsumerNote(AddNewNoteConsumerNoteRequest request, CancellationToken cancellationToken)
    {
        var businessReference = await BusinessHostedService.GetBusinessReferenceFromHttpContext(new GetBusinessReferenceFromHttpContextRequest
        {
            Context = HttpContext
        }, cancellationToken);

        var response = await ComplaintsHostedService.AddConsumerNoteToComplaint(new CreateNewNoteConsumerRequest
        {
            ComplaintReference = request.ComplaintReference,
            BusinessReference = businessReference.BusinessReference,
            NoteText = request.NoteText
        }, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccess)
            return Results.BadRequest(new AddNewNoteConsumerResponse
            {
                IsSuccessful = false,
                Errors = response.Errors
            });

            return Results.Ok(new AddNewNoteConsumerResponse
        {
            IsSuccessful = true,
        });
    }
    // add new note account holder - needs testing
    [HttpPost("AddNoteUser", Name = "AddNoteUser"), BasicAuth]
    [ProducesResponseType(typeof(AddNewNoteUserResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddNewUserNote(AddNewNoteUserRequest request, CancellationToken cancellationToken)
    {
        var user = new AuthedUser(User);

        if (user.Role != RolesEnum.Consumer)
            return Results.Unauthorized();

        var response = await ComplaintsHostedService.AddUserNoteToComplaint(new CreateNewNoteUserRequest
        {
            ComplaintReference = request.ComplaintReference,
            BusinessReference = user.BusinessReference,
            UserReference = user.Reference,
            NoteText = request.NoteText
        }, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessful)
            return Results.BadRequest(new AddNewNoteConsumerResponse
            {
                IsSuccessful = false,
                Errors = response.Errors
            });

        return Results.Ok(new AddNewNoteUserResponse
        {
            IsSuccessful = true,
        });
    }
    // search complaints
    [HttpPost("SearchForComplaint", Name = "SearchForComplaint")]
    [ProducesResponseType(typeof(SearchForComplaintResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> SearchForComplaint(SearchForComplaintRequest request, CancellationToken cancellationToken)
    {
        var businessReference = await BusinessHostedService.GetBusinessReferenceFromHttpContext(new GetBusinessReferenceFromHttpContextRequest
        {
            Context = HttpContext
        },cancellationToken).ConfigureAwait(false);

        var response = await ComplaintsHostedService.SearchComplaints(new ComplaintsSearchRequest
        {
            FirstNote = request.FirstNote,
            ConsumerEmail = request.ConsumerEmail,
            ConsumerPostCode = request.ConsumerPostCode,
            ComplaintReference = request.ComplaintReference,
            IsOpen = request.Is_Open,
            BusinessReference = businessReference.BusinessReference
        }, cancellationToken).ConfigureAwait(false);

        return Results.Ok(new SearchForComplaintResponse
        {
            Complaints = response.Complaints.ConvertAll(x => new Complaint
            {
                Reference = x.Reference,
                FirstMessage = x.FirstMessage,
                TimeOpened = x.TimeOpened,
                LastUpdated = x.LastUpdated,
                IsOpen = x.IsOpen
            }),
            IsSuccess = true
        });
    }
    // close complaint consumer
    // close complaint account holder
    // view complaint consumer
    // view complaint account holder
}
