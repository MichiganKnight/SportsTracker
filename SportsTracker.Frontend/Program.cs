using SportsTracker.Frontend.Config;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<SportsApiOptions>(builder.Configuration.GetSection(SportsApiOptions.SectionName));

builder.Services.AddHttpClient<SportsApiClient>();

builder.Services.AddScoped<IScoreboardApiClient>(provider => provider.GetRequiredService<SportsApiClient>());
builder.Services.AddScoped<IGameApiClient>(provider => provider.GetRequiredService<SportsApiClient>());
builder.Services.AddScoped<IStandingsApiClient>(provider => provider.GetRequiredService<SportsApiClient>());
builder.Services.AddScoped<ITeamApiClient>(provider => provider.GetRequiredService<SportsApiClient>());

builder.Services.AddScoped<IGameCardMapper, GameCardMapper>();
builder.Services.AddScoped<IDashboardMapper, DashboardMapper>();
builder.Services.AddScoped<ILeagueMapper, LeagueMapper>();
builder.Services.AddScoped<INavigationMapper, NavigationMapper>();
builder.Services.AddScoped<IStandingsMapper, StandingsMapper>();
builder.Services.AddScoped<IGameDetailsMapper, GameDetailsMapper>();
builder.Services.AddScoped<IBoxScoreMapper, BoxScoreMapper>();
builder.Services.AddScoped<IPlayByPlayMapper, PlayByPlayMapper>();
builder.Services.AddScoped<ITeamDetailsMapper, TeamDetailsMapper>();
builder.Services.AddScoped<ITeamRosterMapper, TeamRosterMapper>();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();