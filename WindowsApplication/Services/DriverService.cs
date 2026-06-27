using System.IO;
using Newtonsoft.Json;
using WindowsApplication.DTOs;
using WindowsApplication.Models;

namespace WindowsApplication.Services;

public class DriverService(DbHandler db)
{
    const string ConfigDir = "Configs/Drivers.json";
    
    public void LoadData()
    {
        Config[] drivers = JsonConvert.DeserializeObject<Config[]>(File.ReadAllText(ConfigDir))!;

        foreach (Config driver in drivers)
        {
            db.Drivers.Add(new()
            {
                Firstname = driver.Firstname, Lastname = driver.Lastname, Team = "None", Championship = "None"
            });
        }
        
        db.SaveChanges();
    }

    public void UpdateElo(RaceResultModel[] results)
    {
        Dictionary<int, DriverDBDTO> drivers = db.Drivers.ToDictionary(k => k.Id);
        Dictionary<int, int> origElo = db.Drivers.ToDictionary(k => k.Id, v => v.Elo);
        Dictionary<int, int> driverPositions =
            results.ToDictionary(k => k.Position, v => v.DriverId);
        
        for (int i = 0; i < results.Length; i++)
        {
            int position = results[i].Position;
            int driverElo = origElo[results[i].DriverId];
            
            int[] frontElo = Enumerable.Range(1, position - 1).Select(v => origElo[driverPositions[v]]).ToArray();
            int[] backElo = Enumerable.Range(position + 1, results.Length - position).
                Select(v => origElo[driverPositions[v]]).ToArray();
            
            double change = 0; 
            int k = 15;

            if (frontElo.Length > 0)
            {
                double frontMean = frontElo.Average();
                double expected = 1f / (1f + Math.Pow(10f, (frontMean - driverElo) / 400f));
                change += k * (0 - expected);
            }
            
            if (backElo.Length > 0)
            {
                double backMean = backElo.Average();
                double expected = 1f / (1f + Math.Pow(10f, (backMean - driverElo) / 400f));
                change += k * (1 - expected);
            }

            drivers[results[i].DriverId].Elo = driverElo + (int)Math.Round(change);
            db.Drivers.Update(drivers[results[i].DriverId]);
        }

        db.SaveChanges();
    }

    public void EndSeason()
    {
        DriverDBDTO[] drivers = db.Drivers.ToArray();

        foreach (DriverDBDTO driver in drivers)
        {
            driver.Team = "None";
            driver.Championship = "None";
        }
        
        db.Drivers.UpdateRange(drivers);
        db.SaveChanges();
    }
    
    class Config
    {
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
    }
}