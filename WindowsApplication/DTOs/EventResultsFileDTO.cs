namespace WindowsApplication.DTOs;

// ReSharper disable once InconsistentNaming
public class EventResultsFileDTO
{
    public required string Name { get; set; }
    public required string Track { get; set; }
    public required string Layout { get; set; }
    public DateOnly Date { get; set; }
    public required DriverResults[] Results { get; set; }
    
    public class DriverResults
    {
        public int DriverId { get; set; }
        public required string DriverName { get; set; }
        public required string TeamName { get; set; }
        public int DriverPosition { get; set; }
        public int DriverPoints { get; set; }
        public int TeamPoints { get; set; }
    }
}