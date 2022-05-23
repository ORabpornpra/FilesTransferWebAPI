
namespace WebAPIClient
{
    internal class FileUtility
    {
        public static FileMetaData TestMetaData()
        {
            FileMetaData fileMetaData = new FileMetaData()
            {
                FileName = "FileName Test",
                FileExt = "FileExt Test",
                FilesTotal = 5,
                FileNames = new List<string> { "Chunk_1", "Chunk_2", "Chunk_3", "Chunk_4", "Chunk_5" }
            };

            return fileMetaData;
        }
    }
}
