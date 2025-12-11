using FluentNHibernate.Conventions.Helpers;

public class ComplaintsHostedService
{
    public CreateComplaintResponse CreateComplaint(CreateComplaintRequest request)
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

        string consumerPostcode = request.ConsumerPostcode.Replace(" ", "");

        Guid complaintReference = ComplaintsTable.SaveNewComplaintRecord(new CreateComplaintData
        {
            ConsumerEmail = request.ConsumerEmail,
            ConsumerPostcode = consumerPostcode,
            Message = request.FirstMessage,
            BusinessReference = request.BusinessReference
        });

        var notesResponse = AddConsumerNoteToComplaint(new CreateNewNoteConsumerRequest
        {
            ComplaintReference = complaintReference,
            BusinessReference = request.BusinessReference,
            NoteText = request.FirstMessage
        });
        
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
    public CreateNewNoteConsumerResponse AddConsumerNoteToComplaint(CreateNewNoteConsumerRequest request)
    {
        if (ComplaintsTable.CheckIfComplaintExists(request.ComplaintReference, request.BusinessReference))
            return new CreateNewNoteConsumerResponse
            {
                IsSuccess = false,
                Errors = new List<string> { "requested complaint does not exist" }
            };


        NotesTable.SaveNewNote(new CreateNoteData
        {
            ComplaintReference = request.ComplaintReference,
            NoteText = request.NoteText,
            UserReference = Guid.Parse("87de9d86-4079-4b0a-8368-fd037f0fc38f"), // the account reference for the "consumer" in the db
            IsPublic = true
        });

        return new CreateNewNoteConsumerResponse
        {
            IsSuccess = true,
        };
    }
    public CreateNewNoteUserResponse AddUserNoteToComplaint(CreateNewNoteUserRequest request)
    {
        if (ComplaintsTable.CheckIfComplaintExists(request.ComplaintReference, request.BusinessReference))
            return new CreateNewNoteUserResponse
            {
                IsSuccess = false,
                Errors = new List<string> { "requested complaint does not exist" }
            };

        NotesTable.SaveNewNote(new CreateNoteData
        {
            ComplaintReference = request.ComplaintReference,
            NoteText = request.NoteText,
            UserReference = request.UserReference,
            IsPublic = request.IsPublic
        });

        return new CreateNewNoteUserResponse
        {
            IsSuccess = true
        };
    }
    
    public ComplaintsSearchResponse SearchComplaints(ComplaintsSearchRequest request)
    {
        List<string> errors = new();
        if (!String.IsNullOrEmpty(request.ConsumerPostCode))
            request.ConsumerPostCode = request.ConsumerPostCode.Replace(" ", "").ToUpper();
            
        if (!String.IsNullOrEmpty(request.ConsumerEmail))
            request.ConsumerEmail = request.ConsumerEmail.ToLower();

        var response = ComplaintsTable.SearchForComplaints(request);

        return new ComplaintsSearchResponse
        {
            Complaints = response,
            IsSuccess = true
        };
    }
}