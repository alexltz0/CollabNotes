using CollabNotes.Application.Interfaces;
using CollabNotes.Domain.Interfaces;
using CollabNotes.Infrastructure.Persistence;
using CollabNotes.Infrastructure.Repositories;
using CollabNotes.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CollabNotes.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string dataDirectory)
    {
        services.AddDbContext<CollabNotesDbContext>(options =>
            options.UseInMemoryDatabase("CollabNotesDb"));

        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddSingleton<IUserRepository>(_ => new JsonUserRepository(dataDirectory));
        services.AddScoped<IRealtimeNotifier, SignalRRealtimeNotifier>();

        services.AddSignalR();

        return services;
    }
}
