namespace YourAdventure.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string PhotoStr { get; set; } = string.Empty;
        public int CountryFId { get; set; }
    }

    public class FileUploadViewModel
    {
        public IFormFile File { get; set; }
    }

}
