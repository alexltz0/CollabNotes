using Microsoft.AspNetCore.SignalR;

namespace CollabNotes.Infrastructure.Hubs;

public sealed class CollaborationHub : Hub
{
    private static readonly Dictionary<string, HashSet<string>> _noteUsers = new();
    private static readonly object _lock = new();

    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
    }

    public async Task JoinNote(Guid noteId, string userName)
    {
        var groupName = $"note-{noteId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        lock (_lock)
        {
            if (!_noteUsers.ContainsKey(groupName))
                _noteUsers[groupName] = [];

            _noteUsers[groupName].Add(userName);
        }

        await Clients.Group(groupName).SendAsync("UserJoined", noteId, userName, GetUsersInNote(groupName));
    }

    public async Task LeaveNote(Guid noteId, string userName)
    {
        var groupName = $"note-{noteId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        lock (_lock)
        {
            if (_noteUsers.ContainsKey(groupName))
            {
                _noteUsers[groupName].Remove(userName);
                if (_noteUsers[groupName].Count == 0)
                    _noteUsers.Remove(groupName);
            }
        }

        await Clients.Group(groupName).SendAsync("UserLeft", noteId, userName, GetUsersInNote(groupName));
    }

    public async Task SendContentEdit(Guid noteId, string content, string editedBy)
    {
        var groupName = $"note-{noteId}";
        await Clients.OthersInGroup(groupName).SendAsync("ContentEdited", noteId, content, editedBy);
    }

    public async Task SendTitleEdit(Guid noteId, string title, string editedBy)
    {
        var groupName = $"note-{noteId}";
        await Clients.OthersInGroup(groupName).SendAsync("TitleEdited", noteId, title, editedBy);
    }

    public async Task SendCursorPosition(Guid noteId, string userName, int position)
    {
        var groupName = $"note-{noteId}";
        await Clients.OthersInGroup(groupName).SendAsync("CursorMoved", noteId, userName, position);
    }

    private static string[] GetUsersInNote(string groupName)
    {
        lock (_lock)
        {
            return _noteUsers.TryGetValue(groupName, out var users) ? [.. users] : [];
        }
    }
}
