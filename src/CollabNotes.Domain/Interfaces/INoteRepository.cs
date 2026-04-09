using CollabNotes.Domain.Entities;

namespace CollabNotes.Domain.Interfaces;

public interface INoteRepository
{
    Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Note>> GetByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Note>> GetSharedWithUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Note> AddAsync(Note note, CancellationToken cancellationToken = default);
    Task UpdateAsync(Note note, CancellationToken cancellationToken = default);
    Task DeleteAsync(Note note, CancellationToken cancellationToken = default);
}
