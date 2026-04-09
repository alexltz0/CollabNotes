using CollabNotes.Application.DTOs;
using CollabNotes.Application.Mappings;
using CollabNotes.Domain.Interfaces;
using MediatR;

namespace CollabNotes.Application.Notes.Queries.GetNoteById;

public sealed class GetNoteByIdQueryHandler(
    INoteRepository noteRepository,
    IUserRepository userRepository) : IRequestHandler<GetNoteByIdQuery, NoteDto?>
{
    public async Task<NoteDto?> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetByIdAsync(request.Id, cancellationToken);
        if (note is null) return null;

        if (!note.HasAccess(request.RequestingUserId))
            throw new UnauthorizedAccessException("You do not have access to this note.");

        return await note.ToDtoAsync(userRepository, cancellationToken);
    }
}
