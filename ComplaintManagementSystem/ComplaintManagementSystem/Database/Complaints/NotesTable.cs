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
            await session.Transaction.CommitAsync(cancellationToken);
        }
    }
}