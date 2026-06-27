using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace WindowsApplication.DTOs;

// ReSharper disable once InconsistentNaming
public class SeasonDBDTO
{
    public int Id { get; set; }
    public required string Championship { get; set; }
    public required string Team { get; set; }
    public int DriverId { get; set; }
    public int DriverPoints { get; set; }
    public int DriverPosition { get; set; }
    public int TeamPoints { get; set; }
    public int TeamPosition { get; set; }
}