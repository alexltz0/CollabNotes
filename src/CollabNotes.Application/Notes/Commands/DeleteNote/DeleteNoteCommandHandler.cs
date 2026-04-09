using CollabNotes.Application.Interfaces;
using CollabNotes.Domain.Interfaces;
using MediatR;

namespace CollabNotes.Application.Notes.Commands.DeleteNote;

public sealed class DeleteNoteCommandHandler(
    INoteRepository noteRepository,
    IRealtimeNotifier notifier) : IRequestHandler<DeleteNoteCommand>
{
    public async Task Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetByIdAsync(request.NoteId, cancellationToken)
            ?? throw new KeyNotFoundException($"Note with ID '{request.NoteId}' was not found.");

        if (note.OwnerId != request.RequestingUserId)
            throw new UnauthorizedAccessException("Only the owner can delete a note.");

        note.Delete();
        await noteRepository.DeleteAsync(note, cancellationToken);
        await notifier.NoteDeleted(request.NoteId);
    }
}
