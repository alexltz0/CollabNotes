using CollabNotes.Domain.Common;

namespace CollabNotes.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public static User Create(string username, string passwordHash, string displayName)
    {
        return new User
        {
            Username = username.ToLowerInvariant(),
            PasswordHash = passwordHash,
            DisplayName = displayName,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static User Create(string username, string passwordHash, string displayName, Guid id, DateTime createdAt)
    {
        return new User
        {
            Id = id,
            Username = username.ToLowerInvariant(),
            PasswordHash = passwordHash,
            DisplayName = displayName,
            CreatedAt = createdAt
        };
    }

    public bool VerifyPassword(string passwordHash)
        => PasswordHash == passwordHash;
}
