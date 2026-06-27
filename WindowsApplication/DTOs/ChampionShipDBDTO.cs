using System.ComponentModel.DataAnnotations;

namespace WindowsApplication.DTOs;

// ReSharper disable once InconsistentNaming
public class ChampionShipDBDTO
{
    [Key]
    public required string Name { get; set; }
    public required string DriverPoints { get; set; }
    public required string TeamPoints { get; set; }
    public int Prestige { get; set; }
}