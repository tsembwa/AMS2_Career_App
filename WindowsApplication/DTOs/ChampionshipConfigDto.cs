namespace WindowsApplication.DTOs;

public class ChampionshipConfigDto
{
   public enum DurationTypes
   {
      Timed, Lapped
   }
   
   public required string Name { get; set; }
   public DurationTypes DurationType { get; set; }
   public required ChampionshipConfigCar[] Cars { get; set; }
   public required ChampionshipConfigEvent[] Events { get; set; }
   
   public class ChampionshipConfigCar
   {
      public required string Name { get; set; }
      public required string Dlc { get; set; }
      public required string Class { get; set; }
   }
   
   public class ChampionshipConfigEvent
   {
      public required string Name { get; set; }
      public DateOnly Date { get; set; }
      public int Duration { get; set; }
      public required Track MainTrack { get; set; }
      public required Track AlternateTrack { get; set; }
      
      public class Track
      {
         public required string Name { get; set; }
         public required string Layout { get; set; }
         public required string Dlc { get; set; }
      }
   }
}