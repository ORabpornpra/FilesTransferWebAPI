using System.Text;

namespace FileUtility
{
    public class FileProcess
    {
        private static readonly int maxRead = 20000000;// 20MB;
        private static readonly string totalChunk = "Total Chunks: {0}";
        private static readonly string writeFileSucc = "The file {0} has been processed.";
        private static readonly string writeFileErr = "Exception caught in process: {0}";
        private static readonly string fileNameChunk = "Chunks_{0}.raw";

        public static FileMetaData TestMetaData()
        {
            FileMetaData fileMetaData = new FileMetaData()
            {
                FileName = "FileName Test",
                FileExt = "FileExt Test",
                FilesTotal = 5,
                FileNamesList = new List<string> { "Chunk_1", "Chunk_2", "Chunk_3", "Chunk_4", "Chunk_5" }
            };

            return fileMetaData;
        }

        public static async Task CreateFileChunks(string fileName, string saveFilesLoc)
        {
            int numFile = 0;

            using (FileStream fs = File.OpenRead(fileName))
            {
                int totalBytes = (int)fs.Length;
                byte[] bd = new byte[maxRead];
                int bytesRead = 0;
                var totalFiles = Math.Ceiling((double)totalBytes / maxRead);                
                Console.WriteLine(string.Format(totalChunk, totalFiles));

                var tasks = new List<Task>();
                Task[] myTasks = new Task[(int)totalFiles];
                int len = 0;

                while (bytesRead < totalBytes)
                {
                    numFile++;
                    if (numFile == totalFiles)
                    {
                        var lastBytes = totalBytes - bytesRead;
                        bd = new byte[lastBytes];
                        len = fs.Read(bd, 0, lastBytes);
                    }
                    else
                    {
                        len = fs.Read(bd, 0, maxRead);
                    }

                    bytesRead += len;                    
                    var curFileName = string.Format(fileNameChunk, numFile);
                    await WriteToFile(bd, curFileName, saveFilesLoc);
                }
            }
        }

        public static async Task WriteToFile(byte[] buff, string fileName, string saveFilesLoc)
        {
            string newFileName = saveFilesLoc + fileName;

            try
            {
                using (var fs = new FileStream(newFileName, FileMode.Create, FileAccess.Write))
                {
                    await fs.WriteAsync(buff);
                }

                Console.WriteLine(writeFileSucc, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(writeFileErr, ex);
            }

        }
    }

    public class FileMetaData
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }

        public int FilesTotal { get; set; }

        public List<string> FileNamesList { get; set; }
    }
}