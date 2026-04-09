namespace CollabNotes.Domain.Entities;

public class NoteCollaborator
{
    public Guid NoteId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime AddedAt { get; private set; }

    private NoteCollaborator() { }

    public static NoteCollaborator Create(Guid noteId, Guid userId)
    {
        return new NoteCollaborator
        {
            NoteId = noteId,
            UserId = userId,
            AddedAt = DateTime.UtcNow
        };
    }
}
