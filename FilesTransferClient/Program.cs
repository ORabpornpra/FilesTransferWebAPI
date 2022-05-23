using System.Net.Http.Headers;
using System.Configuration;
using System.Net;
using System.Net.Http.Json;

namespace WebAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string defaultContentType = @"application/json";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello OAT World");

            HttpStatusCode postResult = await Program.CreateProductAsync(FileUtility.TestMetaData());
            if (postResult == HttpStatusCode.Created)
            {
                await Program.GetFileMetaDataAsync();
            }
            else
            {
                Console.WriteLine("Failed To Create MetaData File");
            }
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
