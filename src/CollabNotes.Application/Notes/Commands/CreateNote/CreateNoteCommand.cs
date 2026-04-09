using CollabNotes.Application.DTOs;
using MediatR;

namespace CollabNotes.Application.Notes.Commands.CreateNote;

public sealed record CreateNoteCommand(string Title, string Content, string CreatedBy, Guid OwnerId) : IRequest<NoteDto>;
