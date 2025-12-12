using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Mapping;
using System.Security.Cryptography.Xml;

public static class NotesTable
{
    public static async Task SaveNewNote(CreateNoteData data, CancellationToken cancellationToken)
    {
        Guid reference = Guid.NewGuid();

        using (var session = DatabaseConnection.GetSession())
        {
            session.BeginTransaction();
            session.Save(new NotesRecord
            {
                reference = reference,
                complaint_reference = data.ComplaintReference,
                time = DateTime.UtcNow,
                note_text = data.NoteText,
                user_reference = data.UserReference,
                is_public = data.IsPublic
            });
            await session.GetCurrentTransaction().CommitAsync(cancellationToken);
        }
    }
    public static async Task<List<Note>> GetAllNotesForComplaint(Guid complaintReference, bool GetPrivate, CancellationToken cancellationToken)
    {
        using (var session = DatabaseConnection.GetSession())
        {
            var query = session.QueryOver<NotesRecord>()
                .Where(x => x.complaint_reference == complaintReference);
            if (!GetPrivate)
                query.Where(x => x.is_public == true);

            var response = await query.ListAsync(cancellationToken);
            return response.OrderByDescending(x => x.id).Select(x => new Note
            {
                Id = x.id,
                NoteText = x.note_text,
                UserReference = x.user_reference,
                IsPublic = x.is_public,
                TimePosted = x.time
            }).ToList();

        }
    }
}