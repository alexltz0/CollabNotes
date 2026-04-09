using CollabNotes.Domain.Entities;
using CollabNotes.Domain.Interfaces;
using CollabNotes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CollabNotes.Infrastructure.Repositories;

public sealed class NoteRepository(CollabNotesDbContext context) : INoteRepository
{
    public async Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Notes
            .Include(n => n.Collaborators)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Note>> GetByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default)
        => await context.Notes
            .Include(n => n.Collaborators)
            .Where(n => n.OwnerId == ownerId)
            .OrderByDescending(n => n.UpdatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Note>> GetSharedWithUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => await context.Notes
            .Include(n => n.Collaborators)
            .Where(n => n.Collaborators.Any(c => c.UserId == userId))
            .OrderByDescending(n => n.UpdatedAt)
            .ToListAsync(cancellationToken);

    public async Task<Note> AddAsync(Note note, CancellationToken cancellationToken = default)
    {
        await context.Notes.AddAsync(note, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return note;
    }

    public async Task UpdateAsync(Note note, CancellationToken cancellationToken = default)
    {
        context.Notes.Update(note);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Note note, CancellationToken cancellationToken = default)
    {
        context.Notes.Remove(note);
        await context.SaveChangesAsync(cancellationToken);
    }
}
