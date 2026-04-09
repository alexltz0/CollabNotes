using CollabNotes.Application.DTOs;
using CollabNotes.Application.Notes.Commands.AddCollaborator;
using CollabNotes.Application.Notes.Commands.CreateNote;
using CollabNotes.Application.Notes.Commands.DeleteNote;
using CollabNotes.Application.Notes.Commands.RemoveCollaborator;
using CollabNotes.Application.Notes.Commands.UpdateNoteContent;
using CollabNotes.Application.Notes.Commands.UpdateNoteTitle;
using CollabNotes.Application.Notes.Queries.GetAllNotes;
using CollabNotes.Application.Notes.Queries.GetNoteById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CollabNotes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController(IMediator mediator) : ControllerBase
{
    private Guid GetUserId() => Guid.Parse(Request.Headers["X-User-Id"].ToString());

    [HttpGet("my")]
    public async Task<ActionResult<IReadOnlyList<NoteDto>>> GetMyNotes(CancellationToken ct)
    {
        var notes = await mediator.Send(new GetMyNotesQuery(GetUserId()), ct);
        return Ok(notes);
    }

    [HttpGet("shared")]
    public async Task<ActionResult<IReadOnlyList<NoteDto>>> GetSharedNotes(CancellationToken ct)
    {
        var notes = await mediator.Send(new GetSharedNotesQuery(GetUserId()), ct);
        return Ok(notes);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NoteDto>> GetById(Guid id, CancellationToken ct)
    {
        var note = await mediator.Send(new GetNoteByIdQuery(id, GetUserId()), ct);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpPost]
    public async Task<ActionResult<NoteDto>> Create([FromBody] CreateNoteRequest request, CancellationToken ct)
    {
        var userId = GetUserId();
        var note = await mediator.Send(
            new CreateNoteCommand(request.Title, request.Content, request.CreatedBy, userId), ct);
        return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
    }

    [HttpPut("{id:guid}/content")]
    public async Task<ActionResult<NoteDto>> UpdateContent(
        Guid id, [FromBody] UpdateContentRequest request, CancellationToken ct)
    {
        var note = await mediator.Send(
            new UpdateNoteContentCommand(id, request.Content, request.EditedBy, GetUserId()), ct);
        return Ok(note);
    }

    [HttpPut("{id:guid}/title")]
    public async Task<ActionResult<NoteDto>> UpdateTitle(
        Guid id, [FromBody] UpdateTitleRequest request, CancellationToken ct)
    {
        var note = await mediator.Send(
            new UpdateNoteTitleCommand(id, request.Title, request.EditedBy, GetUserId()), ct);
        return Ok(note);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteNoteCommand(id, GetUserId()), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/collaborators")]
    public async Task<ActionResult<NoteDto>> AddCollaborator(
        Guid id, [FromBody] AddCollaboratorRequest request, CancellationToken ct)
    {
        var note = await mediator.Send(
            new AddCollaboratorCommand(id, request.Username, GetUserId()), ct);
        return Ok(note);
    }

    [HttpDelete("{id:guid}/collaborators/{userId:guid}")]
    public async Task<ActionResult<NoteDto>> RemoveCollaborator(
        Guid id, Guid userId, CancellationToken ct)
    {
        var note = await mediator.Send(
            new RemoveCollaboratorCommand(id, userId, GetUserId()), ct);
        return Ok(note);
    }
}

public record CreateNoteRequest(string Title, string Content, string CreatedBy);
public record UpdateContentRequest(string Content, string EditedBy);
public record UpdateTitleRequest(string Title, string EditedBy);
public record AddCollaboratorRequest(string Username);
