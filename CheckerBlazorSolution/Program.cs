using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using CheckerBlazorServer.Data;
using CheckerBlazorServer.CheckerService;
using CheckerBlazorServer.CheckerRepositoryNS;
using Checker.Server.HubNS;
using Microsoft.Extensions.Configuration;
using System;
using CheckerBlazorServer.Database;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<ICheckerService, CheckerService>();
builder.Services.AddScoped<ICheckerRepository, CheckerRepository>();
builder.Services.AddSingleton<TableManager>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.∫
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

