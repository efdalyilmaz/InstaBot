using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace instabot
{
    public class Bot : IDisposable
    {
        private const int DelayForWaitCount = 5;
        private static UserSessionData user;
        private static IInstaApi api;

        public Bot(string userName, string password)
        {
            user = new UserSessionData();
            user.UserName = userName;
            user.Password = password;

            Task login = Login();
            Task.WaitAny(login);
        }

        public async Task Login()
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

        public async void Logout()
        {
            await api.LogoutAsync();
        }

        public async void PullUserPosts(string userName)
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
                if (m != null && m.Caption != null && !String.IsNullOrEmpty(m.Caption.Text) && m.MediaType == InstaMediaType.Image)
                {
                    for (int x = 0; x < m.Images.Count; x++)
                    {
                        if (m.Images[x] != null && m.Images[x].URI != null)
                        {
                            Console.WriteLine(m.Caption.Text);
                            Console.WriteLine(m.Images[x].URI);
                        }
                    }
                }
            }

        }

        public async void PullUserInfo(string userName)
        {
            IResult<InstaUser> userInfo = await api.GetUserAsync(userName);
            writeAllProperties(userInfo);
        }

        public async Task<IResult<InstaUserShortList>> PullUsersFollowers(string userName)
        {
            IResult<InstaUserShortList> userShortList = await api.GetUserFollowersAsync(userName, PaginationParameters.Empty);

            return userShortList;
        }

        public async void MakeFollowRequestToPrivateAccount(string userName)
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

        public async Task FollowUser(long userId)
        {
            await api.FollowUserAsync(userId);
        }

        private void waitForLogin()
        {
            int timeout = 30; // second
            while (!api.IsUserAuthenticated)
            {
                timeout--;
                if (timeout < 0)
                {
                    throw new TimeoutException("Login timeout");
                }

                Task.Delay(1000);
            }
        }

        private void writeAllProperties(object obj)
        {
            string sObj = JsonConvert.SerializeObject(obj);
            JObject parsed = JObject.Parse(sObj);
            foreach (var pair in parsed)
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }

        }

        public void Dispose()
        {
            Logout();
        }
    }
}
