using System.Security.Cryptography.Xml;

public static class NotesTable
{
    public static void SaveNewNote(CreateNoteData data)
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
            session.Transaction.Commit();
        }
    }
}