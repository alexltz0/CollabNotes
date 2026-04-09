using CollabNotes.Application.DTOs;
using CollabNotes.Application.Interfaces;
using CollabNotes.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CollabNotes.Infrastructure.Services;

public sealed class SignalRRealtimeNotifier(
    IHubContext<CollaborationHub> hubContext) : IRealtimeNotifier
{
    public async Task NoteCreated(NoteDto note)
        => await hubContext.Clients.All.SendAsync("NoteCreated", note);

    public async Task NoteUpdated(NoteDto note)
        => await hubContext.Clients.Group($"note-{note.Id}").SendAsync("NoteUpdated", note);

    public async Task NoteDeleted(Guid noteId)
        => await hubContext.Clients.All.SendAsync("NoteDeleted", noteId);

    public async Task UserJoined(Guid noteId, string userName)
        => await hubContext.Clients.Group($"note-{noteId}").SendAsync("UserJoined", noteId, userName);

    public async Task UserLeft(Guid noteId, string userName)
        => await hubContext.Clients.Group($"note-{noteId}").SendAsync("UserLeft", noteId, userName);

    public async Task ContentEdited(Guid noteId, string content, string editedBy)
        => await hubContext.Clients.Group($"note-{noteId}").SendAsync("ContentEdited", noteId, content, editedBy);

    public async Task CollaboratorAdded(NoteDto note, Guid addedUserId)
        => await hubContext.Clients.Group($"user-{addedUserId}").SendAsync("CollaboratorAdded", note);

    public async Task CollaboratorRemoved(Guid noteId, Guid removedUserId)
        => await hubContext.Clients.Group($"user-{removedUserId}").SendAsync("CollaboratorRemoved", noteId);
}
