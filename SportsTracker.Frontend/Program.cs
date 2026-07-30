using SportsTracker.Frontend.Config;
using SportsTracker.Frontend.Mapping;
using SportsTracker.Frontend.Services.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<SportsApiOptions>(builder.Configuration.GetSection(SportsApiOptions.SectionName));

builder.Services.AddHttpClient<ISportsApiClient, SportsApiClient>();

builder.Services.AddScoped<IDashboardMapper, DashboardMapper>();

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