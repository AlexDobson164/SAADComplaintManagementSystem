using ComplaintManagementSystem.Database.Complaints;
using Microsoft.AspNetCore.Mvc;

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
        
        Guid complaintReference = ComplaintsTable.SaveNewComplaintRecord(new CreateComplaintData
        {
            ConsumerEmail = request.ConsumerEmail,
            ConsumerPostcode = request.ConsumerPostcode,
            Message = request.FirstMessage,
            BusinessReference = request.BusinessReference
        });
        
        NotesTable.SaveNewNote(new CreateNoteData
        {
            ComplaintReference = complaintReference,
            NoteText = request.FirstMessage,
            UserReference = Guid.Parse("87de9d86-4079-4b0a-8368-fd037f0fc38f"), // the account reference for the "consumer" in the db
            IsPublic = true
        });

        return new CreateComplaintResponse
        {
            ComplaintReference = complaintReference,
            IsSuccessful = true,
            Errors = errors
        };
    }
}