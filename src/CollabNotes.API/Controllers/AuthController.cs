using CollabNotes.Application.Auth.Commands.Login;
using CollabNotes.Application.Auth.Commands.Register;
using CollabNotes.Application.Auth.Queries;
using CollabNotes.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CollabNotes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResultDto>> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new RegisterCommand(request.Username, request.Password, request.DisplayName), ct);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new LoginCommand(request.Username, request.Password), ct);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> SearchUsers([FromQuery] string q, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
            return Ok(Array.Empty<UserDto>());

        var users = await mediator.Send(new SearchUsersQuery(q), ct);
        return Ok(users);
    }
}

public record RegisterRequest(string Username, string Password, string DisplayName);
public record LoginRequest(string Username, string Password);
