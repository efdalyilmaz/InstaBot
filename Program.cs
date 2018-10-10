using InstaBot.API;
using InstaBot.API.Builder;
using InstaBot.Utils;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Unsplasharp;
using Unsplasharp.Models;

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
            await api.UploadPhotoAsync("");
            await api.Logout();

            return true;
        }

    }
}
