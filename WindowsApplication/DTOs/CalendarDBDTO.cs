namespace WindowsApplication.DTOs;

// ReSharper disable once InconsistentNaming
public class CalendarDBDTO
{
    public int Id { get; set; }
    public required string Championship { get; set; }
    public required string EventName { get; set; }
    public required DateOnly Date { get; set; }
    public int Year { get; set; }
}