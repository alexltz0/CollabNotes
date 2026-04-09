using CollabNotes.Application.DTOs;
using MediatR;

namespace CollabNotes.Application.Notes.Commands.RemoveCollaborator;

public sealed record RemoveCollaboratorCommand(Guid NoteId, Guid CollaboratorUserId, Guid RequestingUserId) : IRequest<NoteDto>;
