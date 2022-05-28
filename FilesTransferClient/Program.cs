using System.Net.Http.Headers;
using System.Configuration;
using System.Net;
using System.Net.Http.Json;
using FileUtility;

namespace WebAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string defaultContentType = @"application/json";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello OAT World");

            HttpStatusCode postResult = await Program.CreateProductAsync(FileProcess.TestMetaData());
            if (postResult == HttpStatusCode.Created)
            {
                await Program.GetFileMetaDataAsync();
            }
            else
            {
                Console.WriteLine("Failed To Create MetaData File");
            }

            //// Testing
            //string filePath = @"c:\temp\2018-05-25 11-02-45.mkv";
            //string savePath = @"C:\temp\tempWrite\";
            //await FileProcess.CreateFileChunks(filePath, savePath);
        }

        private static async Task GetFileMetaDataAsync()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(defaultContentType));

            var stringTask = client.GetStringAsync(ConfigurationManager.AppSettings.Get("FileMetaDataGet"));

            var msg = await stringTask;
            Console.WriteLine(msg);
        }

        private static async Task<HttpStatusCode> CreateProductAsync(FileMetaData fileMetaData)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(ConfigurationManager.AppSettings.Get("FileMetaDataPost"), fileMetaData);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.StatusCode.ToString());

            return response.StatusCode;
        }
    }
}
