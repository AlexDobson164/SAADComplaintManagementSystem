using FluentNHibernate.Mapping;

public class NotesRecord
{
    public virtual int id { get; set; }
    public virtual Guid reference { get; set; }
    public virtual Guid complaint_reference { get; set; }
    public virtual DateTime time { get; set; }
    public virtual string note_text { get; set; }
    public virtual Guid user_reference { get; set; }
    public virtual bool is_public { get; set; }
}
public class NotesMapping : ClassMap<NotesRecord>
{
    public NotesMapping()
    {
        Table("complaints.notes");
        Id(x => x.id)
            .Column("id")
            .GeneratedBy.Sequence("complaints.notes_id_seq");
        Map(x => x.reference);
        Map(x => x.complaint_reference);
        Map(x => x.time);
        Map(x => x.note_text);
        Map(x => x.user_reference);
        Map(x => x.is_public);
    }
}

