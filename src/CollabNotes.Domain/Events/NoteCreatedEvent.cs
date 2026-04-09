using CollabNotes.Domain.Common;

namespace CollabNotes.Domain.Events;

public sealed record NoteCreatedEvent(Guid NoteId, string Title, string CreatedBy) : DomainEvent;
