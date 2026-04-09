using CollabNotes.Application.DTOs;
using MediatR;

namespace CollabNotes.Application.Notes.Commands.UpdateNoteContent;

public sealed record UpdateNoteContentCommand(Guid NoteId, string Content, string EditedBy, Guid RequestingUserId) : IRequest<NoteDto>;
