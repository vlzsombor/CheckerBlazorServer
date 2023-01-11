using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using CheckerBlazorServer.Data;
using CheckerBlazorServer.CheckerService;
using CheckerBlazorServer.CheckerRepositoryNS;
using Checker.Server.HubNS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<ICheckerService, CheckerService>();
builder.Services.AddScoped<ICheckerRepository, CheckerRepository>();
builder.Services.AddSingleton<TableManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<MultiPlayerHub>("/multiPlayerHub");
app.MapFallbackToPage("/_Host");

app.Run();

