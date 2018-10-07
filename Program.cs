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
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        private const int DelayForWaitCount = 5;    

        private static string userName = "";
        private static string password = "";

        private static UserSessionData user;
        private static IInstaApi api;

        static void Main(string[] args)
        {

            Console.Write("UserName : ");
            userName = Console.ReadLine();

            Console.Write("Password : ");
            password = ReadPassword();
            Console.WriteLine("\n--------------");

            user = new UserSessionData();
            user.UserName = userName;
            user.Password = password;

            Login();
            Console.Read();
        }

        private static async Task Login()
        {
            api = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .SetRequestDelay(RequestDelay.FromSeconds(3, 5))
                .Build();

            var loginRequest = await api.LoginAsync();
            if (loginRequest.Succeeded)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine(loginRequest.Info.Message);
            }
        }

        public static async void Logout()
        {
            await api.LogoutAsync();
        }

        public static async void PullUserPosts(string userName)
        {
            IResult<InstaUser> userInfo = await api.GetUserAsync(userName);
            Console.WriteLine(userInfo.Value.FullName);
            Console.WriteLine(userInfo.Value.IsPrivate);
            Console.WriteLine(userInfo.Value.FollowersCount);
            Console.WriteLine(userInfo.Value.FriendshipStatus.ToString());
            Console.WriteLine(userInfo.Value.SocialContext);
            Console.WriteLine(userInfo.Value.UnseenCount);
            
            IResult<InstaMediaList> media = await api.GetUserMediaAsync(userName, PaginationParameters.MaxPagesToLoad(5));
            InstaMediaList mediaList = media.Value;

            for (int i = 0; i < mediaList.Count; i++)
            {
                InstaMedia m = mediaList[i];
                if(m != null && m.Caption != null && !String.IsNullOrEmpty(m.Caption.Text) && m.MediaType == InstaMediaType.Image)
                {
                    for (int x = 0; x < m.Images.Count; x++)
                    {
                        if(m.Images[x] != null && m.Images[x].URI != null)
                        {
                            Console.WriteLine(m.Caption.Text);
                            Console.WriteLine(m.Images[x].URI);
                        }
                    }
                }
            }

        }

        public static async void PullUserInfo(string userName)
        {
            await Login();

            IResult<InstaUser> userInfo = await api.GetUserAsync(userName);
            WriteAllProperties(userInfo);
            
        }

        public static async Task<IResult<InstaUserShortList>> PullUsersFollowers(string userName)
        {
            await Login();

            IResult<InstaUserShortList> userShortList = await api.GetUserFollowersAsync(userName, PaginationParameters.Empty);
            
            return userShortList;
        }

        public static async void MakeFollowRequestToPrivateAccount(string userName)
        {
            IResult<InstaUserShortList> userShortList = await PullUsersFollowers(userName);
            int privateUserCount = userShortList.Value.FindAll(u => u.IsPrivate).Count;
            Console.WriteLine(String.Format("Private User Count : {0}", privateUserCount));
            int wait = DelayForWaitCount;
            foreach (var user in userShortList.Value)
            {

                if (user.IsPrivate)
                {
                    wait--;
                    privateUserCount--;
                    if (wait == 0)
                    {
                        wait = DelayForWaitCount;
                        await api.FollowUserAsync(user.Pk);
                    }
                    else
                    {
                        api.FollowUserAsync(user.Pk);
                    }
                    
                    Console.WriteLine(String.Format("Requested User : {0}, Remaining Count: {1}", user.FullName, privateUserCount));
                    
                    
                }
            }
            
        }

        public static async Task FollowUser(long userId)
        {
            await api.FollowUserAsync(userId);
        }

        public static void WaitForLogin()
        {
            int timeout = 30; // second
            while (!api.IsUserAuthenticated)
            {
                timeout--;
                if (timeout < 0)
                {
                    throw new TimeoutException("Login timeout");
                }
                
                Thread.Sleep(1000);
            }
        }

        public static void WriteAllProperties(object obj)
        {
            string sObj = JsonConvert.SerializeObject(obj);
            JObject parsed = JObject.Parse(sObj);
            foreach (var pair in parsed)
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }

        }

        public static string ReadPassword()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            return pass;
        }
    }
}
