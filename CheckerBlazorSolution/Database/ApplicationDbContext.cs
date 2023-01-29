using System;
using CheckerBlazorServer.Database.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CheckerBlazorServer.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<TestDto> Test { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

}

