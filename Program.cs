using InstaBot.API;
using InstaBot.API.Builder;
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

            IApi api = ApiBuilder.CreateBuilder()
                .SetUser(user.Item1, user.Item2)
                .UseStockApi(true)
                .SetKeys(applicationId, secretKey)
                .Build();

            await api.Login();
            await api.MakeFollowRequestAsync("");
            await api.Logout();

            return true;
        }

    }
}
