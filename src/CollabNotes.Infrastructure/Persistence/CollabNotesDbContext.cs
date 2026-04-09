using CollabNotes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollabNotes.Infrastructure.Persistence;

public class CollabNotesDbContext(DbContextOptions<CollabNotesDbContext> options) : DbContext(options)
{
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<NoteCollaborator> NoteCollaborators => Set<NoteCollaborator>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Content);
            entity.Property(e => e.LastEditedBy).IsRequired().HasMaxLength(100);
            entity.Property(e => e.OwnerId);
            entity.Property(e => e.CreatedAt);
            entity.Property(e => e.UpdatedAt);
            entity.Ignore(e => e.DomainEvents);

            entity.HasMany(e => e.Collaborators)
                  .WithOne()
                  .HasForeignKey(c => c.NoteId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Navigation(e => e.Collaborators)
                  .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<NoteCollaborator>(entity =>
        {
            entity.HasKey(e => new { e.NoteId, e.UserId });
            entity.Property(e => e.AddedAt);
        });
    }
}
