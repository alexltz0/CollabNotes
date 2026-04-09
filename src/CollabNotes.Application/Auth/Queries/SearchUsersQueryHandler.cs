using CollabNotes.Application.DTOs;
using CollabNotes.Domain.Interfaces;
using MediatR;

namespace CollabNotes.Application.Auth.Queries;

public sealed class SearchUsersQueryHandler(
    IUserRepository userRepository) : IRequestHandler<SearchUsersQuery, IReadOnlyList<UserDto>>
{
    public async Task<IReadOnlyList<UserDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.SearchByUsernameAsync(request.Query, cancellationToken);
        return users.Select(u => new UserDto(u.Id, u.Username, u.DisplayName)).ToList();
    }
}
