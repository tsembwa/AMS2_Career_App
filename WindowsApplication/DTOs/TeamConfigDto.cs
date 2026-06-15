namespace WindowsApplication.DTOs;

public class TeamConfigDto
{
    public required string Name { get; set; }
    public required TeamConfigChampionship[] Championships { get; set; }
    
    public class TeamConfigChampionship
    {
        public required string Name { get; set; }
        public required TeamConfigCar[] Cars { get; set; }
        
        public class TeamConfigCar
        {
            public required string Name { get; set; }
            public required string Class { get; set; }
            public int Drivers { get; set; }
        }
    }
}