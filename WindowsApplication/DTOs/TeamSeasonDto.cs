namespace WindowsApplication.DTOs;

public class TeamSeasonDto
{
    public required TeamChampionship[] Championships { get; set; }
    
    public class TeamChampionship
    {
        public required string Name { get; set; }
        public required TeamChampionshipsDriver[] Drivers { get; set; }
        public int Result { get; set; }
        
        public class TeamChampionshipsDriver
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public int FinalResult { get; set; }
            public int FinalPoints { get; set; }
            public required TeamChampionshipCar Car { get; set; }
        }
        
        public class TeamChampionshipCar
        {
            public required string Name { get; set; }
            public int CarNumber { get; set; }
            public required string Class { get; set; }
        }

    }
}