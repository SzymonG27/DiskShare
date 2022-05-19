using DiskShare.Models.FileModels;

namespace DiskShare.Models.ViewModels
{
    public class FileViewModel
    {
        public NewFile newFile { get; set; } = null!;
        public bool isFilesExist { get; set; }
        public bool isTheSameUsrId { get; set; }
        public List<FileInfo> Files { get; set; } = null!;
        public string FileFullName { get; set; } = null!;
        public string DiskUrl { get; set; } = null!;
        public long DiskCapacity { get; set; }

    }
}
