namespace DiskShare.Models.FileModels
{
    public class NewFile
    {
        public string FileDescription { get; set; } = null!;
        public IFormFile formFile { get; set; } = null!;
    }
}
