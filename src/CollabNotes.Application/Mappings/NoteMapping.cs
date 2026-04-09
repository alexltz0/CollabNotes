using CollabNotes.Application.DTOs;
using CollabNotes.Domain.Entities;
using CollabNotes.Domain.Interfaces;

namespace CollabNotes.Application.Mappings;

public static class NoteMapping
{
    public static async Task<NoteDto> ToDtoAsync(this Note note, IUserRepository userRepository, CancellationToken ct = default)
    {
        var owner = await userRepository.GetByIdAsync(note.OwnerId, ct);
        var ownerName = owner?.DisplayName ?? "Unbekannt";

        var collaborators = new List<CollaboratorDto>();
        foreach (var c in note.Collaborators)
        {
            var user = await userRepository.GetByIdAsync(c.UserId, ct);
            if (user is not null)
                collaborators.Add(new CollaboratorDto(user.Id, user.Username, user.DisplayName));
        }

        return new NoteDto(
            note.Id, note.Title, note.Content, note.LastEditedBy,
            note.OwnerId, ownerName, collaborators,
            note.CreatedAt, note.UpdatedAt);
    }
}
