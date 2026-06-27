using System.IO;
using Newtonsoft.Json;
using WindowsApplication.DTOs;
using WindowsApplication.Models;

namespace WindowsApplication.Services;

public class ChampionshipService(DbHandler db, string saveDataDir)
{
    const string ConfigDir = "Configs/Championships.json"; 
    
    readonly string _championshipEventsDir = Path.Combine(saveDataDir, "ChampionshipEvents");
    
    public void LoadData()
    {
        Config[] config = JsonConvert.DeserializeObject<Config[]>(File.ReadAllText(ConfigDir))!;

        if (!Directory.Exists(_championshipEventsDir))
            Directory.CreateDirectory(_championshipEventsDir);
        
        foreach (Config champ in config)
        {
            db.Championships.Add(new()
            {
                Name = champ.Name, DriverPoints = JsonConvert.SerializeObject(champ.DriverPoints),
                TeamPoints = JsonConvert.SerializeObject(champ.TeamPoints), Prestige = champ.Prestige
            });

            List<EventSetUpFileDTO> events = new();

            foreach (EventConfig eventDto in champ.Events)
            {
                events.Add(new()
                {
                    Name = eventDto.Name, Date = eventDto.Date, Duration = eventDto.Duration, 
                    Track = eventDto.MainTrack.Name, Layout = eventDto.MainTrack.Layout,
                    StartTimeI = eventDto.StartTime
                });
            }
            
            File.WriteAllText(Path.Combine(_championshipEventsDir, $"{champ.Name}.json"), 
                JsonConvert.SerializeObject(events, Formatting.Indented));
        }

        db.SaveChanges();
    }

    public void GenerateSeason(int year)
    {
        db.Season.RemoveRange(db.Season);
        db.Calendar.RemoveRange(db.Calendar);
        
        foreach (ChampionShipDBDTO championship in db.Championships)
        {
            EventSetUpFileDTO[] events = JsonConvert.DeserializeObject<EventSetUpFileDTO[]>(
                File.ReadAllText(Path.Combine(_championshipEventsDir, $"{championship.Name}.json")))!;

            string dir  = ChampionshipDir(year, championship.Name);

            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, "events.json"), "[]");
            File.WriteAllText(Path.Combine(dir, "results.json"), "[]");
            
            foreach (EventSetUpFileDTO eventDto in events)
            {
                db.Calendar.Add(new()
                {
                    Championship = championship.Name, Date = eventDto.Date, 
                    EventName = eventDto.Name, Year = year
                });
            }
        }
    }
 
    public EventSetUpFileDTO[] GetEvents(string championship)
    {
        return JsonConvert.DeserializeObject<EventSetUpFileDTO[]>(
            File.ReadAllText(Path.Combine(_championshipEventsDir, $"{championship}.json")))!;
    }

    public EventSetUpFileDTO GetEventSetUp(string championship, string eventName) =>
        GetEvents(championship).First(x => x.Name == eventName);
    
    public void SaveResult(RaceResultModel[] results, EventSetUpFileDTO eventDto, int year, string championship)
    {
        string dir = ChampionshipDir(year, championship);
        string eventResultsDir = Path.Combine(dir, "events.json");
        string finalResultsDir = Path.Combine(dir, "results.json");
        
        List<EventFileDTO> events = JsonConvert.DeserializeObject<List<EventFileDTO>>(File.ReadAllText(eventResultsDir))!;
        
        events.Add(new()
        {
            Name = eventDto.Name, Track = eventDto.Track, Layout = eventDto.Layout, Results = results
        });

        SeasonDBDTO[] drivers = db.Season.Where(x => x.Championship == championship).ToArray();
        Dictionary<int, SeasonDBDTO> driverLookUp = drivers.ToDictionary(x => x.DriverId, x => x);
        
        foreach (RaceResultModel result in results)
        {
            SeasonDBDTO driver = driverLookUp[result.DriverId];

            driver.DriverPoints += result.DriverPoints;
            driver.TeamPoints +=  result.TeamPoints;
        }

        drivers = drivers.OrderByDescending(x => x.DriverPoints).ToArray();

        Dictionary<string, int> teamPositions = drivers.GroupBy(x => x.Team).
            ToDictionary(k => k.Key, v => 
                v.Sum(x => x.TeamPoints));

        string[] sortedTeam = teamPositions.OrderByDescending(x => x.Value).
            Select(x => x.Key).ToArray();
        
        for (int i = 0; i < drivers.Length; i++)
            drivers[i].DriverPosition = i + 1;

        for (int i = 0; i < sortedTeam.Length; i++)
        {
            string team = sortedTeam[i];

            foreach (SeasonDBDTO driver in drivers.Where(x => x.Team == team))
            {
                driver.TeamPosition = i + 1;
                driver.TeamPoints = teamPositions[team];
            }
        }
        
        db.Season.UpdateRange(drivers);
        db.SaveChanges();
        
        File.WriteAllText(eventResultsDir, JsonConvert.SerializeObject(events, Formatting.Indented));
        File.WriteAllText(finalResultsDir, JsonConvert.SerializeObject(drivers, Formatting.Indented));
    }
    
    public RaceResultModel[] SimulateRace(DriverDBDTO[] drivers, ChampionShipDBDTO championship)
    {
        List<RaceResultModel> results = new();

        int[] driverPointsTable = JsonConvert.DeserializeObject<int[]>(championship.DriverPoints)!;
        int[] teamPointsTable = JsonConvert.DeserializeObject<int[]>(championship.TeamPoints)!;
        
        Random rnd = new();

        Dictionary<int, int> driverPositions = new();
        
        for (int i = 0; i < drivers.Length; i++)
        {
            DriverDBDTO driver = drivers[i];
            int position = i + 1;
            driverPositions.TryAdd(driver.Id, position);
            
            foreach (int driverId in driverPositions.Keys)
            {
                if (driverId == driver.Id)
                    continue;
                
                DriverDBDTO driverB = drivers.First(x => x.Id == driverId);
                
                double chance = 1f / (1 + Math.Pow(10, (double)(driverB.Elo - driver.Elo) / 400)); 

                if (rnd.NextDouble() < chance)
                {
                    (driverPositions[driver.Id], driverPositions[driverId]) = 
                        (driverPositions[driverId], driverPositions[driver.Id]);
                }
            }
        }
        foreach (int driverId in driverPositions.Keys)
        {
            int position = driverPositions[driverId];
            int dPoints = position < driverPointsTable.Length  ? driverPointsTable[position - 1] : 0;
            int tPoints = position < teamPointsTable.Length ? teamPointsTable[position - 1] : 0;
            
            results.Add(new()
            {
                DriverId =  driverId, Position = position, 
                Team = drivers.First(x => x.Id == driverId).Team,
                TeamPoints = tPoints, DriverPoints = dPoints
            });
        }
        
        return results.ToArray();
    }

    string ChampionshipDir(int year, string championship) =>
        Path.Combine(saveDataDir, $"Seasons/{year}/Championships/{championship}");
    
    class Config
    {
        public required string Name { get; set; }
        public required string DurationType { get; set; }
        public int Prestige { get; set; }
        public required int[] DriverPoints { get; set; }
        public required int[] TeamPoints { get; set; }
        public required EventConfig[] Events { get; set; } 
    }

    class EventConfig
    {
        public required string Name { get; set; }
        public DateOnly Date { get; set; }
        public int Duration { get; set; }
        public int StartTime { get; set; }
        public required TrackConfig MainTrack { get; set; }
        public required TrackConfig AlternateTrack { get; set; }
    }

    class TrackConfig
    {
        public required string Name { get; set; }
        public required string Layout { get; set; }
        public required string Dlc { get; set; }
    }
}