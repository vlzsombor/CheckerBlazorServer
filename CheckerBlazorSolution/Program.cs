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
using Microsoft.AspNetCore.Identity;
using CheckerBlazorServer.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using CheckerBlazorServer.InitConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<ICheckerService, CheckerService>();
builder.Services.AddScoped<ICheckerRepository, CheckerRepository>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddSingleton<TableManager>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<DbContext, ApplicationDbContext>();


//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//  .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddScoped<AuthenticationStateProvider, NewAuthenticationStateProvider>();

var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<DbContext>();


var app = builder.Build();

//ApplicationDbInitializer.SeedUsers(app.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(), app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());


app.MapControllers();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapHub<MultiPlayerHub>("/multiPlayerHub");
app.MapFallbackToPage("/_Host");

app.Run();

