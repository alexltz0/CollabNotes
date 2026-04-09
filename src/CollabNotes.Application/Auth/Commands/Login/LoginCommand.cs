using CollabNotes.Application.DTOs;
using MediatR;

namespace CollabNotes.Application.Auth.Commands.Login;

public sealed record LoginCommand(string Username, string Password) : IRequest<AuthResultDto>;
