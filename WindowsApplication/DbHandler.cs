using Microsoft.EntityFrameworkCore;
using WindowsApplication.DTOs;

namespace WindowsApplication;

public class DbHandler : DbContext
{
    public DbSet<TeamDBDTO> Teams { get; set; }
    public DbSet<EntryDBDTO> Entries { get; set; }
    public DbSet<ChampionShipDBDTO> Championships { get; set; }
    public DbSet<DriverDBDTO>  Drivers { get; set; }
    public DbSet<SeasonDBDTO> Season { get; set; }
    public DbSet<CalendarDBDTO> Calendar { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=AMS2Career.db");
    }

}