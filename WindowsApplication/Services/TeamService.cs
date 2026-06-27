using System.IO;
using Newtonsoft.Json;
using WindowsApplication.DTOs;

namespace WindowsApplication.Services;

public class TeamService(DbHandler db, string saveDataDir)
{
   private readonly string _configDir = "Configs/Teams.json";
   
   public void LoadData()
   {
      TeamConfig[] teams = JsonConvert.DeserializeObject<TeamConfig[]>(File.ReadAllText(_configDir))!;

      foreach (TeamConfig team in teams)
      {
         db.Teams.Add(new()
         {
            Name = team.Name, Reputation = team.Reputation
         });

         foreach (EntryConfig entry in team.Entries)
         {
            foreach (CarConfig car in entry.Cars)
            {
               db.Entries.Add(new()
               {
                  Car = car.Name, Level = car.Level, CarNumber = car.CarNumber, Drivers = car.Drivers,
                  Championship = entry.Name, Class = car.Class, Team = team.Name, TeamReputation = team.Reputation
               });
            }
         }
      }

      db.SaveChanges();
   }

   public void GenerateSeason(int year)
   {
      EntryDBDTO[] entries = db.Entries.OrderByDescending(x => x.Level).
            ThenByDescending(x => x.TeamReputation).ToArray();
      
      DriverDBDTO[] drivers = db.Drivers.OrderByDescending(x => x.Elo).ToArray();
      
      int currentDriver = 0;
      
      foreach (EntryDBDTO entry in entries)
      {
         for (int i = 0; i < entry.Drivers; i++)
         {
            DriverDBDTO driver = drivers[currentDriver];
            driver.Team = entry.Team;
            driver.Championship = entry.Championship;

            db.Drivers.Update(driver);
            
            currentDriver++;
         
            db.Season.Add(new()
            {
               Championship = entry.Championship, DriverId = driver.Id, Team = entry.Team
            });
         }
      }
      
      db.SaveChanges();
   }
   
   class TeamConfig
   {
      public required string Name { get; set; }
      public int Reputation { get; set; }
      public required EntryConfig[] Entries { get; set; }   
   } 
   class EntryConfig
   {
      public required string Name { get; set; }
      public required CarConfig[] Cars { get; set; }
   }
   class CarConfig
   {
      public required string Name { get; set; }
      public required string Class { get; set; }
      public int Level { get; set; }
      public int CarNumber { get; set; }
      public int Drivers { get; set; }
   }
}