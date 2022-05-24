
using System.Text;

namespace WebAPIClient
{
    internal class FileUtility
    {
        private static readonly int maxRead = 20000000;// 20MB;

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

        public static async Task FileProcess(string fileName)
        {
            int numFile = 0;

            using (FileStream fs = File.OpenRead(fileName))
            {
                int totalBytes = (int)fs.Length;
                byte[] bd = new byte[maxRead];
                int bytesRead = 0;
                var totalFiles = Math.Ceiling((double)totalBytes / maxRead);
                Console.WriteLine($"Total Chunks: {totalFiles}");
                UTF8Encoding temp = new UTF8Encoding(true);

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
                    await WriteToFile(bd, $"Chunks_{numFile}.raw");
                }
            }
        }

        public static async Task WriteToFile(byte[] buff, string fileName)
        {
            string path = @"C:\temp\tempWrite\" + fileName;

            try
            {
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await fs.WriteAsync(buff);
                }

                Console.WriteLine("The file {0} has been processed.", fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
            }

        }
    }
}
