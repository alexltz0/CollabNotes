using CollabNotes.Application.DTOs;
using MediatR;

namespace CollabNotes.Application.Notes.Queries.GetNoteById;

public sealed record GetNoteByIdQuery(Guid Id, Guid RequestingUserId) : IRequest<NoteDto?>;
