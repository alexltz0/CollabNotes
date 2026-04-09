namespace CollabNotes.Application.DTOs;

public sealed record UserDto(Guid Id, string Username, string DisplayName);

public sealed record AuthResultDto(Guid Id, string Username, string DisplayName, string Token);
