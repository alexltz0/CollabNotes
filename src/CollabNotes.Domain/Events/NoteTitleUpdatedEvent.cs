using CollabNotes.Domain.Common;

namespace CollabNotes.Domain.Events;

public sealed record NoteTitleUpdatedEvent(Guid NoteId, string Title, string EditedBy) : DomainEvent;
