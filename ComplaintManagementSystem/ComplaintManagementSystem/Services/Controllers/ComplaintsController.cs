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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RegisterComplaintResponse> RegisterComplaint(RegisterComplaintRequest request)
    {
        Guid businessReference = BusinessHostedService.GetBusinessReferenceFromHttpContext(new GetBusinessReferenceFromHttpContextRequest
        {
            Context = HttpContext
        }).BusinessReference;

        var response = ComplaintsHostedService.CreateComplaint(new CreateComplaintRequest
        {
            ConsumerEmail = request.ConsumerEmail.ToLower(),
            ConsumerPostcode = request.ConsumerPostCode.ToUpper(),
            FirstMessage = request.NoteText,
            BusinessReference = businessReference
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
        return Ok(new RegisterComplaintResponse
        {
            ComplaintReference = response.ComplaintReference,
            IsSuccessful = response.IsSuccessful,
            Errors = response.Errors
        });
    }
    // for these 2 methods, check if the complaint has the correct business ref
    // needs testing
    [HttpPost("AddNoteConsumer", Name = "AddNoteConsumer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<AddNewNoteConsumerResponse> AddNewConsumerNote(AddNewConsumerNoteRequest request)
    {
        Guid businessReference = BusinessHostedService.GetBusinessReferenceFromHttpContext(new GetBusinessReferenceFromHttpContextRequest
        {
            Context = HttpContext
        }).BusinessReference;

        var response = ComplaintsHostedService.AddConsumerNoteToComplaint(new CreateNewNoteConsumerRequest
        {
            ComplaintReference = request.ComplaintReference,
            BusinessReference = businessReference,
            NoteText = request.NoteText
        });

        if (!response.IsSuccess)
            return BadRequest(new AddNewNoteConsumerResponse
            {
                IsSuccess = false,
                Errors = response.Errors
            });

        return Ok(new AddNewNoteConsumerResponse
        {
            IsSuccess = true,
        });
    }
    // add new note account holder - needs testing
    [HttpPost("AddNoteUser", Name = "AddNoteUser"), BasicAuth]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<AddNewNoteUserResponse> AddNewUserNote(AddNewNoteUserRequest request)
    {
        var user = new AuthedUser(User);

        if (user.Role != RolesEnum.Consumer)
            return Unauthorized("User is not authorized to add a note using this endpoint");

        var response = ComplaintsHostedService.AddUserNoteToComplaint(new CreateNewNoteUserRequest
        {
            ComplaintReference = request.ComplaintReference,
            BusinessReference = user.BusinessReference,
            UserReference = user.Reference,
            NoteText = request.NoteText
        });

        if (!response.IsSuccess)
            return BadRequest(new AddNewNoteConsumerResponse
            {
                IsSuccess = false,
                Errors = response.Errors
            });

        return Ok(new AddNewNoteUserResponse
        {
            IsSuccess = true,
        });
    }
    // search complaints
    [HttpPost("SearchForComplaint", Name = "SearchForComplaint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SearchForComplaintResponse> SearchForComplaint(SearchForComplaintRequest request)
    {
        Guid businessReference = BusinessHostedService.GetBusinessReferenceFromHttpContext(new GetBusinessReferenceFromHttpContextRequest
        {
            Context = HttpContext
        }).BusinessReference;

        var response = ComplaintsHostedService.SearchComplaints(new ComplaintsSearchRequest
        {
            FirstNote = request.FirstNote,
            ConsumerEmail = request.ConsumerEmail,
            ConsumerPostCode = request.ConsumerPostCode,
            ComplaintReference = request.ComplaintReference,
            IsOpen = request.Is_Open,
            BusinessReference = businessReference
        });

        return new SearchForComplaintResponse
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
        };
    }
    // close complaint consumer
    // close complaint account holder
    // view complaint consumer
    // view complaint account holder
}
