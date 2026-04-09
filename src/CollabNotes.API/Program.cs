using CollabNotes.API.Middleware;
using CollabNotes.Application;
using CollabNotes.Infrastructure;
using CollabNotes.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices();

var dataDir = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
builder.Services.AddInfrastructureServices(dataDir);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(_ => true);
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<CollaborationHub>("/hubs/collaboration");
app.MapFallbackToFile("index.html");

app.Run();
