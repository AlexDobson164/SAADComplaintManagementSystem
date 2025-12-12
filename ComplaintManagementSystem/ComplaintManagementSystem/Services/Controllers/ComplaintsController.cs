using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

[ApiController]
[Route("[controller]")]
public class ComplaintsController : ControllerBase
{
    AccountsHostedService AccountsHostedService = new AccountsHostedService();
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
    
    [HttpPost("AddNoteUser", Name = "AddNoteUser"), BasicAuth]
    [ProducesResponseType(typeof(AddNewNoteUserResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddNewUserNote(AddNewNoteUserRequest request, CancellationToken cancellationToken)
    {
        var user = new AuthedUser(User);

        if (user.Role == RolesEnum.Consumer)
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
    
    [HttpPost("SearchForComplaint", Name = "SearchForComplaint")]
    [ProducesResponseType(typeof(SearchForComplaintResponse),StatusCodes.Status200OK)]
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
    [HttpPost("CloseComplaintConsumer", Name = "CloseComplaintConsumer")]
    [ProducesResponseType(typeof(CloseComplaintConsumerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> CloseComplaintConsumer(CloseComplaintConsumerRequest request, CancellationToken cancellationToken)
    {
        var businessReference = await BusinessHostedService.GetBusinessReferenceFromHttpContext(new GetBusinessReferenceFromHttpContextRequest
        {
            Context = HttpContext
        }, cancellationToken);

        var response = await ComplaintsHostedService.ConsumerCloseComplaint(new ConsumerCloseComplaintRequest
        {
           ComplaintReference = request.ComplaintReference,
           BusinessReference = businessReference.BusinessReference,
           ConsumerEmail = request.ConsumerEmail,
           ConsumerPostcode = request.ConsumerPostcode,
           Feedback = request.Feedback,
        }, cancellationToken);

        if (!response.IsSuccessful)
        {
            if (response.ErrorCode == StatusCodes.Status404NotFound)
                return Results.NotFound(new CloseComplaintConsumerResponse
                {
                    IsSuccessful = false,
                    Errors = new List<string> { response.ErrorMessage }
                });
            return Results.Unauthorized();
        }

        EmailHostedService.SendComplaintClosedEmail(new SendComplaintClosedEmailRequest
        {
            ReceivingEmail = request.ConsumerEmail,
            SendingEmail = "Complaints@ComplaintsManagementSystem.com",
            EmailText = $"""
            Hi,
            We are contacting you to let you know that your complaint has been closed
            If this wasn't done by you, please contact us immediately and give us the information below so that we can get this resolved:
                - your email
                - your postcode

            This information will help us find your complaint.

            Thank you
            Complaint Management System
            """
        });

        return Results.Ok(new CloseComplaintConsumerResponse
        {
            IsSuccessful = true
        });
    }

    // close complaint account holder
    [HttpPost("CloseComplaintUser", Name = "CloseComplaintUser"), BasicAuth]
    [ProducesResponseType(typeof(CloseComplaintUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> CloseComplaintUser(CloseComplaintUserRequest request, CancellationToken cancellationToken)
    {

        var user = new AuthedUser(User);

        if (user.Role == RolesEnum.Consumer)
            return Results.Unauthorized();

        var response = await ComplaintsHostedService.UserCloseComplaint(new UserCloseComplaintRequest
        {
            ComplaintReference = request.ComplaintReference,
            BusinessReference = user.BusinessReference,
            ConsumerEmail = request.ConsumerEmail,
            ConsumerPostcode = request.ConsumerPostcode,
            UserReference = user.Reference,
            Feedback = request.Feedback,
        }, cancellationToken);

        if (!response.IsSuccessful)
        {
            if (response.ErrorCode == StatusCodes.Status404NotFound)
                return Results.NotFound(new CloseComplaintUserResponse
                {
                    IsSuccessful = false,
                    Errors = new List<string> { response.ErrorMessage }
                });
            if (response.ErrorCode != StatusCodes.Status400BadRequest)
                return Results.BadRequest(new CloseComplaintUserResponse
                {
                    IsSuccessful = false,
                    Errors = new List<string> { response.ErrorMessage }
                });
        }

        EmailHostedService.SendComplaintClosedEmail(new SendComplaintClosedEmailRequest
        {
            ReceivingEmail = response.ConsumerEmail,
            SendingEmail = "Complaints@ComplaintsManagementSystem.com",
            EmailText = $"""
            Hi,
            We are contacting you to let you know that your complaint has been closed by a Help Desk Agent.
            If you feel that your complaint was not appropriately responded to,
            please contact us immediately and give us the information below so that we can get this resolved and re open the complaint:
                - your email
                - your postcode
           
            This information will help us find your complaint.

            Thank you
            Complaint Management System
            """
        });

        return Results.Ok(new CloseComplaintUserResponse
        {
            IsSuccessful = true
        });
    }

    // view complaint consumer
    [HttpPost("ViewComplaintConsumer", Name = "ViewComplaintConsumer")]
    [ProducesResponseType(typeof(ViewComplaintConsumerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> ViewComplaintConsumer(ViewComplaintConsumerRequest request, CancellationToken cancellationToken)
    {
        var businessReference = await BusinessHostedService.GetBusinessReferenceFromHttpContext(new GetBusinessReferenceFromHttpContextRequest
        {
            Context = HttpContext
        }, cancellationToken);

        var response = await ComplaintsHostedService.ViewComplaint(new ViewComplaintRequest
        {
            ComplaintReference = request.ComplaintReference,
            BusinessReference = businessReference.BusinessReference,
            GetPrivate = false
        }, cancellationToken);

        if (!response.IsSuccessful)
            return Results.NotFound(new ViewComplaintConsumerResponse
            {
                IsSucessful = false,
                Errors = new List<string>() { "Complaint could not be found"}
            });
        return Results.Ok(new ViewComplaintConsumerResponse
        {
            IsSucessful = true,
            IsOpen = response.IsOpen,
            Notes = response.Notes.ConvertAll(x => new ComplaintNoteForConsumer
            {
                NoteText = x.NoteText,
                SenderName = AccountsHostedService.GetNameByUserRef(new GetNameByUserReferenceRequest
                {
                    UserReference = x.UserReference
                }, cancellationToken).Result.Name,
                SenderReference = x.UserReference,
                TimeNoteSent = x.TimePosted
            })
        });
    }

    // view complaint account holder
    [HttpPost("ViewComplaintUser", Name = "ViewComplaintUser")]
    [ProducesResponseType(typeof(ViewComplaintUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> ViewComplaintUser(ViewComplaintUserRequest request, CancellationToken cancellationToken)
    {
        var user = new AuthedUser(User);

        var response = await ComplaintsHostedService.ViewComplaint(new ViewComplaintRequest
        {
            ComplaintReference = request.ComplaintReference,
            BusinessReference = user.BusinessReference,
            GetPrivate = true
        }, cancellationToken);

        if (!response.IsSuccessful)
            return Results.NotFound(new ViewComplaintUserResponse
            {
                IsSucessful = false,
                Errors = new List<string>() { "Complaint could not be found" }
            });
        return Results.Ok(new ViewComplaintUserResponse
        {
            IsSucessful = true,
            IsOpen = response.IsOpen,
            Notes = response.Notes.ConvertAll(x => new ComplaintNoteForUser
            {
                NoteText = x.NoteText,
                SenderName = AccountsHostedService.GetNameByUserRef(new GetNameByUserReferenceRequest
                {
                    UserReference = x.UserReference
                }, cancellationToken).Result.Name,
                SenderReference = x.UserReference,
                TimeNoteSent = x.TimePosted,
                IsPublic = x.IsPublic
            })
        });
    }

    
    // assign support engineer to task
    // get assigned complaints
}
