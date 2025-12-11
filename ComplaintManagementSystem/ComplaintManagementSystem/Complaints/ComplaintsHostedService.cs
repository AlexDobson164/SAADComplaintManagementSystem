using FluentNHibernate.Conventions.Helpers;

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
}