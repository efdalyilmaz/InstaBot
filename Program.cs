using InstaBot.API;
using InstaBot.API.Builder;
using InstaBot.API.Logger;
using InstaBot.API.Processors;
using InstaBot.API.Utils;
using InstaBot.Utils;
using System;
using System.Threading.Tasks;

namespace InstaBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var result = Task.Run(MainAsync).GetAwaiter().GetResult();
            if (result)
                return;

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        public static async Task<bool> MainAsync()
        {
            var user = ConsoleUtils.GetUserInfo();

            string applicationId = System.Configuration.ConfigurationManager.AppSettings.Get("ApplicationId");
            string secretKey = System.Configuration.ConfigurationManager.AppSettings.Get("SecretKey");
            string downloadPath = System.Configuration.ConfigurationManager.AppSettings.Get("DownloadPath");

            
            IApi api = ApiBuilder.CreateBuilder()
                .UseLogger(new ConsoleLogger())
                .SetUser(user.Item1, user.Item2)
                .UseStockApi(true)
                .SetKeys(applicationId, secretKey)
                .Build();

            string searchCategory = "holiday";
            IDownloadProcessor downloadProcessor = new DownloadProcessor(new ConsoleLogger(), FileUtils.GetFullDirectory(downloadPath, searchCategory));
            
            await api.Login();
            await api.UploadPhotoAsync(searchCategory, downloadProcessor);
            await api.Logout();

            return true;
        }

    }
}
