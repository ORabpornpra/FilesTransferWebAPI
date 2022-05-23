using System.Net.Http.Headers;
using System.Configuration;

namespace WebAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string defaultContentType = @"application/json";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello OAT World");

            await Program.GetFileMetaDataAsync();
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
    }
}
