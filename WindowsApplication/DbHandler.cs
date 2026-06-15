using System.Data;
using Microsoft.EntityFrameworkCore;
using WindowsApplication.DTOs;

namespace WindowsApplication;

public class DbHandler : DbContext
{
    public DbSet<DriverDto> Drivers { get; set; }
    public DbSet<TeamDto> Teams { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=AMS2Career.db");
    }
}