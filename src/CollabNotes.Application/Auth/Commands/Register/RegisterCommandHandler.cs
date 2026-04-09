using System.Security.Cryptography;
using System.Text;
using CollabNotes.Application.DTOs;
using CollabNotes.Domain.Entities;
using CollabNotes.Domain.Interfaces;
using MediatR;

namespace CollabNotes.Application.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IUserRepository userRepository) : IRequestHandler<RegisterCommand, AuthResultDto>
{
    public async Task<AuthResultDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsAsync(request.Username, cancellationToken))
            throw new InvalidOperationException($"Username '{request.Username}' is already taken.");

        var passwordHash = HashPassword(request.Password);
        var user = User.Create(request.Username, passwordHash, request.DisplayName);
        await userRepository.AddAsync(user, cancellationToken);

        return new AuthResultDto(user.Id, user.Username, user.DisplayName, user.Id.ToString());
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
