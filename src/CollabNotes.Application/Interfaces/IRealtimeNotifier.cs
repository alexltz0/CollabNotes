using CollabNotes.Application.DTOs;

namespace CollabNotes.Application.Interfaces;

public interface IRealtimeNotifier
{
    Task NoteCreated(NoteDto note);
    Task NoteUpdated(NoteDto note);
    Task NoteDeleted(Guid noteId);
    Task UserJoined(Guid noteId, string userName);
    Task UserLeft(Guid noteId, string userName);
    Task ContentEdited(Guid noteId, string content, string editedBy);
    Task CollaboratorAdded(NoteDto note, Guid addedUserId);
    Task CollaboratorRemoved(Guid noteId, Guid removedUserId);
}
