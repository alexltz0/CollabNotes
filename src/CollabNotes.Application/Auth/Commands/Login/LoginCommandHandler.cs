using System.Security.Cryptography;
using System.Text;
using CollabNotes.Application.DTOs;
using CollabNotes.Domain.Interfaces;
using MediatR;

namespace CollabNotes.Application.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository) : IRequestHandler<LoginCommand, AuthResultDto>
{
    public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid username or password.");

        var passwordHash = HashPassword(request.Password);
        if (!user.VerifyPassword(passwordHash))
            throw new UnauthorizedAccessException("Invalid username or password.");

        return new AuthResultDto(user.Id, user.Username, user.DisplayName, user.Id.ToString());
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
