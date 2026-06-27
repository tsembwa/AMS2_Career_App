namespace WindowsApplication.DTOs;

// ReSharper disable once InconsistentNaming
public class DriverDBDTO
{
    public int Id { get; set; }
    public int Elo { get; set; } = 100;
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required string Team { get; set; }
    public required string Championship { get; set; }
}