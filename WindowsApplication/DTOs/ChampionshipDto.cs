using static WindowsApplication.DTOs.ChampionshipConfigDto;

namespace WindowsApplication.DTOs;

public class ChampionshipDto
{
    public required string Name { get; set; }
    public required DurationTypes DurationType { get; set; }
    public required ChampionshipConfigCar[] Cars { get; set; }
    public required ChampionshipTeam[] Teams { get; set; }
    public required ChampionshipEvent[] Events { get; set; }
    
    
    
    public class ChampionshipEvent
    {
        public required string Name { get; set; }
        public DateOnly Date { get; set; }
        public int Duration { get; set; }
        public required string Track { get; set; }
        public required string Layout { get; set; }
        public ChampionshipResult[]? Result { get; set; }
    }

    public class ChampionshipTeam
    {
        public required string Name { get; set; }
        public required ChampionshipDriver[] Drivers { get; set; }
    }

    public class ChampionshipDriver
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Car { get; set; }
        public required string Class { get; set; }
    }
    
    public class ChampionshipResult
    {
        public bool Dnf { get; set; }
        public int Position { get; set; }
        public int DriverPoints { get; set; }
        public int TeamPoints { get; set; }
        public required DriverDto Driver { get; set; }
        public required TeamDto Team { get; set; }
    }
}