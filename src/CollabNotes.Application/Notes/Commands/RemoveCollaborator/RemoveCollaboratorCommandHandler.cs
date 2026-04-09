using CollabNotes.Application.DTOs;
using CollabNotes.Application.Interfaces;
using CollabNotes.Application.Mappings;
using CollabNotes.Domain.Interfaces;
using MediatR;

namespace CollabNotes.Application.Notes.Commands.RemoveCollaborator;

public sealed class RemoveCollaboratorCommandHandler(
    INoteRepository noteRepository,
    IUserRepository userRepository,
    IRealtimeNotifier notifier) : IRequestHandler<RemoveCollaboratorCommand, NoteDto>
{
    public async Task<NoteDto> Handle(RemoveCollaboratorCommand request, CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetByIdAsync(request.NoteId, cancellationToken)
            ?? throw new KeyNotFoundException($"Note with ID '{request.NoteId}' was not found.");

        if (note.OwnerId != request.RequestingUserId)
            throw new UnauthorizedAccessException("Only the owner can remove collaborators.");

        note.RemoveCollaborator(request.CollaboratorUserId);
        await noteRepository.UpdateAsync(note, cancellationToken);

        await notifier.CollaboratorRemoved(request.NoteId, request.CollaboratorUserId);

        return await note.ToDtoAsync(userRepository, cancellationToken);
    }
}
