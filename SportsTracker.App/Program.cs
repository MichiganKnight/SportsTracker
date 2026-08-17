using SportsTracker.App.Config;
using SportsTracker.App.Extensions;
using SportsTracker.App.Hubs;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSportsTrackerServices(builder.Configuration);
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("https://localhost:7110").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.Configure<SportsApiOptions>(builder.Configuration.GetSection(SportsApiOptions.SectionName));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("Frontend");

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<ScoreboardHub>("/scoreboardHub");

app.Run();