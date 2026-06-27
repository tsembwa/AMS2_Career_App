using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WindowsApplication.DTOs;

// ReSharper disable once InconsistentNaming
public class TeamDBDTO
{
    [Key]
    public required string Name { get; set; }
    public int Reputation { get; set; }
}