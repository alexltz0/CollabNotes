using CollabNotes.Application.DTOs;
using CollabNotes.Application.Interfaces;
using CollabNotes.Application.Mappings;
using CollabNotes.Domain.Interfaces;
using MediatR;

namespace CollabNotes.Application.Notes.Commands.UpdateNoteContent;

public sealed class UpdateNoteContentCommandHandler(
    INoteRepository noteRepository,
    IUserRepository userRepository,
    IRealtimeNotifier notifier) : IRequestHandler<UpdateNoteContentCommand, NoteDto>
{
    public async Task<NoteDto> Handle(UpdateNoteContentCommand request, CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetByIdAsync(request.NoteId, cancellationToken)
            ?? throw new KeyNotFoundException($"Note with ID '{request.NoteId}' was not found.");

        if (!note.HasAccess(request.RequestingUserId))
            throw new UnauthorizedAccessException("You do not have access to this note.");

        note.UpdateContent(request.Content, request.EditedBy);
        await noteRepository.UpdateAsync(note, cancellationToken);

        var dto = await note.ToDtoAsync(userRepository, cancellationToken);
        await notifier.NoteUpdated(dto);

        return dto;
    }
}
