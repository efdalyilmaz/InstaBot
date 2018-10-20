using InstaBot.API.Logger;
using InstaBot.API.Models;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InstaBot.API.Services
{
    public class InstaService : IInstaService
    {
        private readonly IInstaApi instaApi;
        private readonly ILogger logger;
        private readonly UserSessionData user;

        public InstaService(ILogger logger, string userName, string password)
        {
            this.logger = logger;

            user = new UserSessionData();
            user.UserName = userName;
            user.Password = password;

            instaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(user)
                    .UseLogger(new DebugLogger(LogLevel.Info))
                    .SetRequestDelay(RequestDelay.FromSeconds(1, 3))
                    .Build();

            Task.Run(Login).Wait();
        }

        public async Task Login()
        {
            var loginRequest = await instaApi.LoginAsync();
            if (loginRequest.Succeeded)
            {
                logger.Write("Success");
            }
            else
            {
                logger.Write(loginRequest.Info.Message);
            }
        }

        public async Task Logout()
        {
            await instaApi.LogoutAsync();
        }

        public async Task LikeMediaAsync(string mediaId)
        {
            validateInstaClient();
            validateLoggedIn();

            await instaApi.LikeMediaAsync(mediaId);
        }

        public async Task<UserInfo> GetCurrentUserInfo()
        {
            validateInstaClient();
            validateLoggedIn();

            IResult<InstaCurrentUser> user = await instaApi.GetCurrentUserAsync();

            return user.Value.ToUserInfo();
        }

        public async Task<List<Media>> GetTagFeedAsync(string hashtag, int maxPageToLoad = 10)
        {
            IResult<InstaTagFeed> tagFeeds = await instaApi.GetTagFeedAsync(hashtag, PaginationParameters.MaxPagesToLoad(maxPageToLoad));

            return tagFeeds.Value.Medias.ToMediaList();
        }

        public async Task<List<UserInfo>> GetUserFollowers(string userName, int maxPageToLoad = 10)
        {
            validateInstaClient();
            validateLoggedIn();

            IResult<InstaUserShortList> userShortList = await instaApi.GetUserFollowersAsync(userName, PaginationParameters.MaxPagesToLoad(maxPageToLoad));

            return userShortList.Value.ToUserInfoList();
        }

        public async Task<List<UserInfo>> GetCurrentUserFollowings(int maxPageToLoad = 10)
        {
            validateInstaClient();
            validateLoggedIn();

            IResult<InstaUserShortList> userShortList = await instaApi.GetUserFollowingAsync(user.UserName, PaginationParameters.MaxPagesToLoad(maxPageToLoad));


            return userShortList.Value.ToUserInfoList();
        }

        public async Task FollowUserAsync(long userId)
        {
            validateInstaClient();
            validateLoggedIn();

            IResult<InstaFriendshipStatus> friendshipStatus = await instaApi.FollowUserAsync(userId);
        }

        public async Task UploadPhotoAsync(string fullpath, string caption)
        {
            var mediaImage = new InstaImage
            {
                Height = 1080,
                Width = 1080,
                URI = new Uri(Path.GetFullPath(fullpath), UriKind.Absolute).LocalPath
            };

            var result = await instaApi.UploadPhotoAsync(mediaImage, caption);
            logger.Write(result.Succeeded
                ? $"Media created: {result.Value.Pk}, {result.Value.Caption}"
                : $"Unable to upload photo: {result.Info.Message}");
        }

        #region private part


        private void validateLoggedIn()
        {
            if (!instaApi.IsUserAuthenticated)
                throw new ArgumentException("User must be authenticated");
        }

        private void validateInstaClient()
        {
            if (instaApi == null)
                throw new ArgumentException("Insta Client is null");
        }

        #endregion
    }
}
