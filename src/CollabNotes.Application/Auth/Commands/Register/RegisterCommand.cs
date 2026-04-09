using CollabNotes.Application.DTOs;
using MediatR;

namespace CollabNotes.Application.Auth.Commands.Register;

public sealed record RegisterCommand(string Username, string Password, string DisplayName) : IRequest<AuthResultDto>;
