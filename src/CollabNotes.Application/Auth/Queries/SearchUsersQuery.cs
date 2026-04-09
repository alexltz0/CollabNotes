using CollabNotes.Application.DTOs;
using MediatR;

namespace CollabNotes.Application.Auth.Queries;

public sealed record SearchUsersQuery(string Query) : IRequest<IReadOnlyList<UserDto>>;
