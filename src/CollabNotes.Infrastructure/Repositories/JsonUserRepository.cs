using System.Text.Json;
using CollabNotes.Domain.Entities;
using CollabNotes.Domain.Interfaces;

namespace CollabNotes.Infrastructure.Repositories;

public sealed class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private List<UserRecord> _cache = [];
    private bool _loaded;

    public JsonUserRepository(string dataDirectory)
    {
        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, "users.json");
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var records = await LoadAsync(cancellationToken);
        var record = records.FirstOrDefault(r => r.Id == id);
        return record?.ToDomain();
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var records = await LoadAsync(cancellationToken);
        var record = records.FirstOrDefault(r =>
            r.Username.Equals(username.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase));
        return record?.ToDomain();
    }

    public async Task<IReadOnlyList<User>> SearchByUsernameAsync(string query, CancellationToken cancellationToken = default)
    {
        var records = await LoadAsync(cancellationToken);
        var q = query.ToLowerInvariant();
        return records
            .Where(r => r.Username.Contains(q, StringComparison.OrdinalIgnoreCase)
                     || r.DisplayName.Contains(q, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .Select(r => r.ToDomain())
            .ToList();
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var records = await LoadAsync(cancellationToken);
            records.Add(UserRecord.FromDomain(user));
            await SaveAsync(records, cancellationToken);
            return user;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<bool> ExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        var records = await LoadAsync(cancellationToken);
        return records.Any(r =>
            r.Username.Equals(username.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase));
    }

    private async Task<List<UserRecord>> LoadAsync(CancellationToken ct)
    {
        if (_loaded) return _cache;

        if (!File.Exists(_filePath))
        {
            _cache = [];
            _loaded = true;
            return _cache;
        }

        var json = await File.ReadAllTextAsync(_filePath, ct);
        _cache = JsonSerializer.Deserialize<List<UserRecord>>(json, JsonOpts) ?? [];
        _loaded = true;
        return _cache;
    }

    private async Task SaveAsync(List<UserRecord> records, CancellationToken ct)
    {
        _cache = records;
        var json = JsonSerializer.Serialize(records, JsonOpts);
        await File.WriteAllTextAsync(_filePath, json, ct);
    }

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private sealed record UserRecord(Guid Id, string Username, string PasswordHash, string DisplayName, DateTime CreatedAt)
    {
        public static UserRecord FromDomain(User user)
            => new(user.Id, user.Username, user.PasswordHash, user.DisplayName, user.CreatedAt);

        public User ToDomain()
            => User.Create(Username, PasswordHash, DisplayName, Id, CreatedAt);
    }
}
