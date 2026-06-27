namespace WindowsApplication.DTOs;

// ReSharper disable once InconsistentNaming
public class EventSetUpFileDTO
{
    public int Duration { get; set; }
    public int StartTimeI { get; set; }
    public required DateOnly Date { get; set; }
    public required string Name { get; set; }
    public required string Track { get; set; }
    public required string Layout { get; set; }
}