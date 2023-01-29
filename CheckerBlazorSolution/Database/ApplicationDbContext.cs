using System;
using CheckerBlazorServer.Database.Dtos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CheckerBlazorServer.Database;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<TestDto> Test { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

}