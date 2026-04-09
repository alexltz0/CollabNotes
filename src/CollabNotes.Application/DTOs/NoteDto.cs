namespace CollabNotes.Application.DTOs;

public sealed record NoteDto(
    Guid Id,
    string Title,
    string Content,
    string LastEditedBy,
    Guid OwnerId,
    string OwnerName,
    List<CollaboratorDto> Collaborators,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record CollaboratorDto(Guid UserId, string Username, string DisplayName);
