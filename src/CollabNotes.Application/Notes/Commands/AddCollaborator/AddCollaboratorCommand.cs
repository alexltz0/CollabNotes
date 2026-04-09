using CollabNotes.Application.DTOs;
using MediatR;

namespace CollabNotes.Application.Notes.Commands.AddCollaborator;

public sealed record AddCollaboratorCommand(Guid NoteId, string Username, Guid RequestingUserId) : IRequest<NoteDto>;
