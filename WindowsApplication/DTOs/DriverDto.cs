namespace WindowsApplication.DTOs;

public class DriverDto
{
    public int Id { get; set; }
    public int Elo { get; set; } = 100;
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required string Team { get; set; } = "None";
}