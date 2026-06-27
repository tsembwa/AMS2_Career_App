namespace WindowsApplication.Models;

public class RaceResultModel
{
    public int DriverId { get; set; }
    public required string Team { get; set; }
    public int Position { get; set; }
    public int DriverPoints { get; set; }
    public int TeamPoints { get; set; }
}