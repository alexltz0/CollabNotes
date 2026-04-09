using CollabNotes.Application.DTOs;
using CollabNotes.Application.Interfaces;
using CollabNotes.Application.Mappings;
using CollabNotes.Domain.Entities;
using CollabNotes.Domain.Interfaces;
using MediatR;

namespace CollabNotes.Application.Notes.Commands.CreateNote;

public sealed class CreateNoteCommandHandler(
    INoteRepository noteRepository,
    IUserRepository userRepository,
    IRealtimeNotifier notifier) : IRequestHandler<CreateNoteCommand, NoteDto>
{
    public async Task<NoteDto> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = Note.Create(request.Title, request.Content, request.CreatedBy, request.OwnerId);
        await noteRepository.AddAsync(note, cancellationToken);

        var dto = await note.ToDtoAsync(userRepository, cancellationToken);
        await notifier.NoteCreated(dto);

        return dto;
    }
}
