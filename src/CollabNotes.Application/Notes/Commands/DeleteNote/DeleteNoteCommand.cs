using MediatR;

namespace CollabNotes.Application.Notes.Commands.DeleteNote;

public sealed record DeleteNoteCommand(Guid NoteId, Guid RequestingUserId) : IRequest;
