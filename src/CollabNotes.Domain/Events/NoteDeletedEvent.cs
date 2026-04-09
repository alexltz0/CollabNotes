using CollabNotes.Domain.Common;

namespace CollabNotes.Domain.Events;

public sealed record NoteDeletedEvent(Guid NoteId) : DomainEvent;
