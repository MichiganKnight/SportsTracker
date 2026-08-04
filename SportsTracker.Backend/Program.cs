using SportsTracker.Backend.Extensions;
using SportsTracker.Backend.Hubs;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("https://localhost:7110").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddSportsTrackerServices(builder.Configuration);

builder.Services.AddSignalR();

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();

app.MapHub<ScoreboardHub>("/scoreboardHub");

app.Run();