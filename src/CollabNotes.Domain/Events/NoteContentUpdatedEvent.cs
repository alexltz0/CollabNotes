using CollabNotes.Domain.Common;

namespace CollabNotes.Domain.Events;

public sealed record NoteContentUpdatedEvent(Guid NoteId, string Content, string EditedBy) : DomainEvent;
