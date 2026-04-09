using CollabNotes.Application.DTOs;
using MediatR;

namespace CollabNotes.Application.Notes.Queries.GetAllNotes;

public sealed record GetMyNotesQuery(Guid UserId) : IRequest<IReadOnlyList<NoteDto>>;
public sealed record GetSharedNotesQuery(Guid UserId) : IRequest<IReadOnlyList<NoteDto>>;
