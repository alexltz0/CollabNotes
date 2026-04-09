using CollabNotes.Application.DTOs;
using CollabNotes.Application.Mappings;
using CollabNotes.Domain.Interfaces;
using MediatR;

namespace CollabNotes.Application.Notes.Queries.GetAllNotes;

public sealed class GetMyNotesQueryHandler(
    INoteRepository noteRepository,
    IUserRepository userRepository) : IRequestHandler<GetMyNotesQuery, IReadOnlyList<NoteDto>>
{
    public async Task<IReadOnlyList<NoteDto>> Handle(GetMyNotesQuery request, CancellationToken cancellationToken)
    {
        var notes = await noteRepository.GetByOwnerAsync(request.UserId, cancellationToken);
        var dtos = new List<NoteDto>();
        foreach (var note in notes)
            dtos.Add(await note.ToDtoAsync(userRepository, cancellationToken));
        return dtos;
    }
}

public sealed class GetSharedNotesQueryHandler(
    INoteRepository noteRepository,
    IUserRepository userRepository) : IRequestHandler<GetSharedNotesQuery, IReadOnlyList<NoteDto>>
{
    public async Task<IReadOnlyList<NoteDto>> Handle(GetSharedNotesQuery request, CancellationToken cancellationToken)
    {
        var notes = await noteRepository.GetSharedWithUserAsync(request.UserId, cancellationToken);
        var dtos = new List<NoteDto>();
        foreach (var note in notes)
            dtos.Add(await note.ToDtoAsync(userRepository, cancellationToken));
        return dtos;
    }
}
