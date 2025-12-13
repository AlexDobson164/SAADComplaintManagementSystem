using FluentNHibernate.Conventions.Helpers;
using Org.BouncyCastle.Asn1.Mozilla;

public class ComplaintsHostedService
{
    public async Task<CreateComplaintResponse> CreateComplaint(CreateComplaintRequest request, CancellationToken cancellationToken)
    {
        List<string> errors = new List<string>();
        if (!Validation.ValidateEmail(request.ConsumerEmail))
            errors.Add("Invalid Email");

        if (!Validation.ValidatePostcode(request.ConsumerPostcode))
            errors.Add("Invalid Postcode");

        if (errors.Count != 0)
            return new CreateComplaintResponse
            {
                IsSuccessful = false,
                Errors = errors
            };

        string consumerPostcode = request.ConsumerPostcode.Replace(" ", "").ToUpper();
        string consumerEmail = request.ConsumerEmail.ToLower();

        var complaintReference = await ComplaintsTable.SaveNewComplaintRecord(new CreateComplaintData
        {
            ConsumerEmail = consumerEmail,
            ConsumerPostcode = consumerPostcode,
            Message = request.FirstMessage,
            BusinessReference = request.BusinessReference
        }, cancellationToken);

        var notesResponse = await AddConsumerNoteToComplaint(new CreateNewNoteConsumerRequest
        {
            ComplaintReference = complaintReference,
            BusinessReference = request.BusinessReference,
            NoteText = request.FirstMessage
        }, cancellationToken);
        
        errors.AddRange(notesResponse.Errors);
        if (errors.Count != 0) // this should never be hit but just incase
            return new CreateComplaintResponse
            {
                IsSuccessful = false,
                Errors = errors
            };

        return new CreateComplaintResponse
        {
            ComplaintReference = complaintReference,
            IsSuccessful = true,
            Errors = errors
        };
    }
    public async Task<CreateNewNoteConsumerResponse> AddConsumerNoteToComplaint(CreateNewNoteConsumerRequest request, CancellationToken cancellationToken)
    {
        if (!await ComplaintsTable.CheckIfComplaintExists(request.ComplaintReference, request.BusinessReference, cancellationToken))
            return new CreateNewNoteConsumerResponse
            {
                IsSuccess = false,
                Errors = new List<string> { "requested complaint does not exist" }
            };


        await NotesTable.SaveNewNote(new CreateNoteData
        {
            ComplaintReference = request.ComplaintReference,
            NoteText = request.NoteText,
            UserReference = Guid.Parse("87de9d86-4079-4b0a-8368-fd037f0fc38f"), // the account reference for the "consumer" in the db
            IsPublic = true
        }, cancellationToken);

        await ComplaintsTable.LastUpdated(request.ComplaintReference, request.BusinessReference, cancellationToken);

        return new CreateNewNoteConsumerResponse
        {
            IsSuccess = true,
        };
    }
    public async Task<CreateNewNoteUserResponse> AddUserNoteToComplaint(CreateNewNoteUserRequest request, CancellationToken cancellationToken)
    {
        if (!await ComplaintsTable.CheckIfComplaintExists(request.ComplaintReference, request.BusinessReference, cancellationToken))
            return new CreateNewNoteUserResponse
            {
                IsSuccessful = false,
                Errors = new List<string> { "requested complaint does not exist" }
            };

        await NotesTable.SaveNewNote(new CreateNoteData
        {
            ComplaintReference = request.ComplaintReference,
            NoteText = request.NoteText,
            UserReference = request.UserReference,
            IsPublic = request.IsPublic
        }, cancellationToken);

        await ComplaintsTable.LastUpdated(request.ComplaintReference, request.BusinessReference, cancellationToken);

        return new CreateNewNoteUserResponse
        {
            IsSuccessful = true
        };
    }
    
    public async Task<ComplaintsSearchResponse> SearchComplaints(ComplaintsSearchRequest request, CancellationToken cancellationToken)
    {
        List<string> errors = new();
        if (!String.IsNullOrEmpty(request.ConsumerPostCode))
            request.ConsumerPostCode = request.ConsumerPostCode.Replace(" ", "").ToUpper();
            
        if (!String.IsNullOrEmpty(request.ConsumerEmail))
            request.ConsumerEmail = request.ConsumerEmail.ToLower();

        var response = await ComplaintsTable.SearchForComplaints(request, cancellationToken);

        return new ComplaintsSearchResponse
        {
            Complaints = response,
            IsSuccess = true
        };
    }

    public async Task<ConsumerCloseComplaintResponse> ConsumerCloseComplaint(ConsumerCloseComplaintRequest request, CancellationToken cancellationToken)
    {
        if (!await ComplaintsTable.CheckIfComplaintExists(request.ComplaintReference, request.BusinessReference, cancellationToken))
            return new ConsumerCloseComplaintResponse
            {
                IsSuccessful = false,
                ErrorCode = StatusCodes.Status404NotFound,
                ErrorMessage = "Complaint does not exist"
            };

        if (!await ComplaintsTable.CloseComplaint(new CloseComplaint
        {
            ComplaintReference = request.ComplaintReference,
            BusinessReference = request.BusinessReference,
            ConsumerEmail = request.ConsumerEmail,
            ConsumerPostcode = request.ConsumerPostcode,
            UserReference = Guid.Parse("87de9d86-4079-4b0a-8368-fd037f0fc38f"), // the account reference for the "consumer" in the db
            CloseReason = "Closed By Consumer"

        }, cancellationToken))
            return new ConsumerCloseComplaintResponse
            {
                IsSuccessful = false,
                ErrorCode = StatusCodes.Status401Unauthorized,
                ErrorMessage = "Provided details do not match the details provided when opening the complaint"
            };

        await ComplaintsTable.LastUpdated(request.ComplaintReference, request.BusinessReference, cancellationToken);

        if (!String.IsNullOrEmpty(request.Feedback))
            await AddUserNoteToComplaint(new CreateNewNoteUserRequest
            {
                ComplaintReference = request.ComplaintReference,
                BusinessReference = request.BusinessReference,
                UserReference = Guid.Parse("87de9d86-4079-4b0a-8368-fd037f0fc38f"), // the account reference for the "consumer" in the db
                NoteText = request.Feedback,
                IsPublic = false,
            }, cancellationToken);
        return new ConsumerCloseComplaintResponse
        {
            IsSuccessful = true,
        };
    }
    public async Task<UserCloseComplaintResponse> UserCloseComplaint(UserCloseComplaintRequest request, CancellationToken cancellationToken)
    {
        //nneed to make a new save function that doesnt take consumer info
        if (!await ComplaintsTable.CheckIfComplaintExists(request.ComplaintReference, request.BusinessReference, cancellationToken))
            return new UserCloseComplaintResponse
            {
                IsSuccessful = false,
                ErrorCode = StatusCodes.Status404NotFound,
                ErrorMessage = "Complaint does not exist"
            };

        var lastUpdated = await ComplaintsTable.CheckWhenLastUpdated(request.ComplaintReference, request.BusinessReference, cancellationToken);
        var difference = DateTime.UtcNow - lastUpdated;

        if (!String.IsNullOrEmpty(request.Reason))
        {
            var response = await ComplaintsTable.CloseComplaint(new CloseComplaint
            {
                ComplaintReference = request.ComplaintReference,
                BusinessReference = request.BusinessReference,
                ConsumerEmail = request.ConsumerEmail,
                ConsumerPostcode = request.ConsumerPostcode,
                UserReference = request.UserReference,
                CloseReason = request.Reason

            }, cancellationToken);

            if (response == false)
                return new UserCloseComplaintResponse
                {
                    IsSuccessful = false,
                    ErrorCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Provided details do not match the details provided when opening the complaint"
                };

            if (!String.IsNullOrEmpty(request.Feedback))
                await AddUserNoteToComplaint(new CreateNewNoteUserRequest
                {
                    ComplaintReference = request.ComplaintReference,
                    BusinessReference = request.BusinessReference,
                    UserReference = Guid.Parse("87de9d86-4079-4b0a-8368-fd037f0fc38f"), // the account reference for the "consumer" in the db
                    NoteText = request.Feedback,
                    IsPublic = false,
                }, cancellationToken);

            return new UserCloseComplaintResponse
            {
                IsSuccessful = true,
                ConsumerEmail = request.ConsumerEmail
            };
        }
        if (difference.TotalDays >= 14)
        {
            var response = await ComplaintsTable.CloseComplaintWithoutConsumerInfo(new CloseComplaintWithoutConsumerInfoRequest
            {
                ComplaintReference = request.ComplaintReference,
                BusinessReference = request.BusinessReference,
                UserReference = request.UserReference,
                CloseReason = "Closed by Help Desk Agent after 14 days of inactivity on the complaint"
            }, cancellationToken);
            return new UserCloseComplaintResponse
            {
                IsSuccessful = true,
                ConsumerEmail = response.ConsumerEmail
            };
        }
        return new UserCloseComplaintResponse
        {
            IsSuccessful = false,
            ErrorCode = StatusCodes.Status400BadRequest,
            ErrorMessage = "Did not provide a reason or days since last update has not reached 14 yet"
        };
    }
    public async Task<ViewComplaintResponse> ViewComplaint(ViewComplaintRequest request, CancellationToken cancellationToken)
    {
        if (!await ComplaintsTable.CheckIfComplaintExists(request.ComplaintReference, request.BusinessReference, cancellationToken))
            return new ViewComplaintResponse
            {
                IsSuccessful = false,
                ErrorCode = StatusCodes.Status404NotFound,
                ErrorMessage = "Complaint does not exist"
            };

        var isOpen = ComplaintsTable.IsComplaintOpen(request.ComplaintReference, request.BusinessReference, cancellationToken);

        var notes = NotesTable.GetAllNotesForComplaint(request.ComplaintReference, request.GetPrivate, cancellationToken);

        return new ViewComplaintResponse
        {
            Notes = notes.Result,
            IsOpen = isOpen.Result,
            IsSuccessful = true
        };
    }

    public async Task<AssignSupportEngineerToComplaintResponse> AssignSupportEngineer(AssignSupportEngineerToComplaintRequest request, CancellationToken cancellationToken)
    {
        if (!await ComplaintsTable.CheckIfComplaintExists(request.ComplaintReference, request.BusinessReference, cancellationToken))
            return new AssignSupportEngineerToComplaintResponse
            {
                IsSuccessful = false,
                ErrorCode = StatusCodes.Status404NotFound,
                Error = "Complaint Not Found"
            };

        if (await ComplaintAssignmentTable.IsUserAssigned(request.UserReference, request.ComplaintReference, cancellationToken))
            return new AssignSupportEngineerToComplaintResponse
            {
                IsSuccessful = false,
                ErrorCode = StatusCodes.Status400BadRequest,
                Error = "Engineer is already assigned to complaint"
            };

        await ComplaintAssignmentTable.AssignUser(request.UserReference, request.ComplaintReference, cancellationToken);
        return new AssignSupportEngineerToComplaintResponse
        {
            IsSuccessful = true
        };
    }

    public async Task<UnassignSupportEngineerFromComplaintResponse> UnassignSupportEngineer(UnassignSupportEngineerFromComplaintRequest request, CancellationToken cancellationToken)
    {
        if (!await ComplaintsTable.CheckIfComplaintExists(request.ComplaintReference, request.BusinessReference, cancellationToken))
            return new UnassignSupportEngineerFromComplaintResponse
            {
                IsSuccessful = false,
                ErrorCode = StatusCodes.Status404NotFound,
                Error = "Complaint Not Found"
            };

        if (!await ComplaintAssignmentTable.IsUserAssigned(request.UserReference, request.ComplaintReference, cancellationToken))
            return new UnassignSupportEngineerFromComplaintResponse
            {
                IsSuccessful = false,
                ErrorCode = StatusCodes.Status400BadRequest,
                Error = "User is not assigned to complaint"
            };

        await ComplaintAssignmentTable.UnassignUser(request.UserReference, request.ComplaintReference, cancellationToken);
        return new UnassignSupportEngineerFromComplaintResponse
        {
            IsSuccessful = true
        };
    }

    public async Task<GetAllComplaintsForUserResponse> GetAllComplaintsForUser(GetAllComplaintsForUserRequest request, CancellationToken cancellationToken)
    {
        var complaintReferences = await ComplaintAssignmentTable.GetAllComplaintReferencesForUserRef(request.UserReference, cancellationToken);

        List<ComplaintsInfo> complaints = new();
        foreach (Guid reference in complaintReferences)
        {
            var record = await ComplaintsTable.GetComplaintByReference(reference, request.BusinessReference, cancellationToken);
            complaints.Add(new ComplaintsInfo
            {
                Reference = record.Reference,
                FirstMessage = record.FirstMessage,
                CreatedAt = record.TimeOpened,
                LastUpdated = record.LastUpdated
            });
        }

        return new GetAllComplaintsForUserResponse { 
            Complaints = complaints,
            IsSucessful = true
        };
    }
}