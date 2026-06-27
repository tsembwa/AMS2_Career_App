namespace WindowsApplication.DTOs;

// ReSharper disable once InconsistentNaming
public class EntryDBDTO
{
    public int Id { get; set; }
    public required string Championship { get; set; }
    public required string Team { get; set; }
    public required string Car { get; set; }
    public required string Class { get; set; }
    public int TeamReputation { get; set; }
    public int CarNumber { get; set; }
    public int Drivers { get; set; }
    public int Level { get; set; }
}