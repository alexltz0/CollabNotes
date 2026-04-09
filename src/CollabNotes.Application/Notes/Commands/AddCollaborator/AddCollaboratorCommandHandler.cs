using CollabNotes.Application.DTOs;
using CollabNotes.Application.Interfaces;
using CollabNotes.Application.Mappings;
using CollabNotes.Domain.Interfaces;
using MediatR;

namespace CollabNotes.Application.Notes.Commands.AddCollaborator;

public sealed class AddCollaboratorCommandHandler(
    INoteRepository noteRepository,
    IUserRepository userRepository,
    IRealtimeNotifier notifier) : IRequestHandler<AddCollaboratorCommand, NoteDto>
{
    public async Task<NoteDto> Handle(AddCollaboratorCommand request, CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetByIdAsync(request.NoteId, cancellationToken)
            ?? throw new KeyNotFoundException($"Note with ID '{request.NoteId}' was not found.");

        if (note.OwnerId != request.RequestingUserId)
            throw new UnauthorizedAccessException("Only the owner can add collaborators.");

        var user = await userRepository.GetByUsernameAsync(request.Username, cancellationToken)
            ?? throw new KeyNotFoundException($"User '{request.Username}' was not found.");

        note.AddCollaborator(user.Id);
        await noteRepository.UpdateAsync(note, cancellationToken);

        var dto = await note.ToDtoAsync(userRepository, cancellationToken);
        await notifier.CollaboratorAdded(dto, user.Id);

        return dto;
    }
}
