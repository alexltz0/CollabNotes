using CollabNotes.Application.DTOs;
using MediatR;

namespace CollabNotes.Application.Notes.Commands.UpdateNoteTitle;

public sealed record UpdateNoteTitleCommand(Guid NoteId, string Title, string EditedBy, Guid RequestingUserId) : IRequest<NoteDto>;
