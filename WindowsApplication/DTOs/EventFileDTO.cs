using WindowsApplication.Models;

namespace WindowsApplication.DTOs;

// ReSharper disable once InconsistentNaming
public class EventFileDTO
{
    public required string Name { get; set; }
    public required string Track { get; set; }
    public required string Layout { get; set; }
    public required RaceResultModel[] Results { get; set; }
    
}