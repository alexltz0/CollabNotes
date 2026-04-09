using CollabNotes.Domain.Common;
using CollabNotes.Domain.Events;

namespace CollabNotes.Domain.Entities;

public class Note : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public string LastEditedBy { get; private set; } = string.Empty;
    public Guid OwnerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<NoteCollaborator> _collaborators = [];
    public IReadOnlyCollection<NoteCollaborator> Collaborators => _collaborators.AsReadOnly();

    private Note() { }

    public static Note Create(string title, string content, string createdBy, Guid ownerId)
    {
        var note = new Note
        {
            Title = title,
            Content = content,
            LastEditedBy = createdBy,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        note.AddDomainEvent(new NoteCreatedEvent(note.Id, title, createdBy));
        return note;
    }

    public void UpdateContent(string content, string editedBy)
    {
        Content = content;
        LastEditedBy = editedBy;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new NoteContentUpdatedEvent(Id, content, editedBy));
    }

    public void UpdateTitle(string title, string editedBy)
    {
        Title = title;
        LastEditedBy = editedBy;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new NoteTitleUpdatedEvent(Id, title, editedBy));
    }

    public void AddCollaborator(Guid userId)
    {
        if (_collaborators.Any(c => c.UserId == userId))
            return;
        if (userId == OwnerId)
            return;

        _collaborators.Add(NoteCollaborator.Create(Id, userId));
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveCollaborator(Guid userId)
    {
        var collaborator = _collaborators.FirstOrDefault(c => c.UserId == userId);
        if (collaborator is not null)
        {
            _collaborators.Remove(collaborator);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public bool HasAccess(Guid userId)
        => OwnerId == userId || _collaborators.Any(c => c.UserId == userId);

    public void Delete()
    {
        AddDomainEvent(new NoteDeletedEvent(Id));
    }
}
