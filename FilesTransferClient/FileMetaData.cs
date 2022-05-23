
namespace WebAPIClient
{
    internal class FileMetaData
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }

        public int FilesTotal { get; set; }

        public List<string> FileNames { get; set; }
    }
}
