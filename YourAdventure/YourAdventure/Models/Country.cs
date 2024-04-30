namespace YourAdventure.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string Geojson { get; set; }= string.Empty;
    }
}