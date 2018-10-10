using InstaBot.API.Filter;
using InstaBot.API.Logger;
using InstaBot.API.Utils;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unsplasharp;
using Unsplasharp.Models;

namespace InstaBot.API
{
    internal class Api : IApi
    {
        private readonly IInstaApi instaClient;
        private readonly UnsplasharpClient stockClient;
        private readonly ILogger logger;

        public Api(ILogger logger, string userName, string password)
        {
            UserSessionData user = new UserSessionData();
            user.UserName = userName;
            user.Password = password;

            this.logger = logger;

            instaClient = InstaApiBuilder.CreateBuilder()
                    .SetUser(user)
                    .UseLogger(new DebugLogger(LogLevel.Info))
                    .SetRequestDelay(RequestDelay.FromSeconds(3, 5))
                    .Build();
        }

        public Api(ILogger logger, string userName, string password, string applicationId, string secretKey) : this(logger, userName, password)
        {
            stockClient = new UnsplasharpClient(applicationId, secretKey);
        }

        public async Task Login()
        {
            var loginRequest = await instaClient.LoginAsync();
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
            await instaClient.LogoutAsync();
        }

        public async Task MakeFollowRequestAsync(string userName, IFilter<InstaUserShort> filter = null)
        {
            validateInstaClient();
            validateLoggedIn();

            IResult<InstaUserShortList> userShortList = await instaClient.GetUserFollowersAsync(userName, PaginationParameters.Empty);
            filter = filter ?? FollowerFilter.DefaultFilter();
            var filtered = filter.Apply(userShortList.Value);

            for (int i = 0; i < filtered.Count; i++)
            {
                await instaClient.FollowUserAsync(filtered[i].Pk);
                logger.Write($"Requested UserName : {filtered[i].UserName}, Remaining User {filtered.Count - i - 1}");
            }

        }

        public async Task UploadPhotoAsync(string stockCategoryName)
        {
            validateInstaClient();
            validateLoggedIn();
            validateStockClient();

            List<Photo> photoList = await stockClient.SearchPhotos(stockCategoryName, 1, 30);
            FileUtils.DownloadAllPhotos(photoList);

            int uploadedPhoto = 1;
            logger.Write(String.Format("Downloaded photo count {0}", FileUtils.ListOfDownloadedPhoto));
            foreach (var photo in FileUtils.ListOfDownloadedPhoto)
            {

                const string imagesSubdirectory = @"D:\unsplash\holiday";
                string filePath = imagesSubdirectory + @"\" + photo.Id + ".jpg";

                string caption = photo.Description + " \r\n \r\n";
                caption += "Thanx to " + photo.User.Name + " \r\n\r\n\r\n";
                caption += "#holiday #travel #trip \r\n";

                await uploadPhotoAsync(filePath, caption);

                logger.Write(String.Format("{0}. uploaded. PhotoId : {1} ", uploadedPhoto, photo.Id));
                uploadedPhoto++;

            }
        }

        private async Task uploadPhotoAsync(string fullpath, string caption)
        {
            var mediaImage = new InstaImage
            {
                Height = 1080,
                Width = 1080,
                URI = new Uri(Path.GetFullPath(fullpath), UriKind.Absolute).LocalPath
            };

            var result = await instaClient.UploadPhotoAsync(mediaImage, caption);
            logger.Write(result.Succeeded
                ? $"Media created: {result.Value.Pk}, {result.Value.Caption}"
                : $"Unable to upload photo: {result.Info.Message}");
        }

        #region private part

        private void validateLoggedIn()
        {
            if (!instaClient.IsUserAuthenticated)
                throw new ArgumentException("User must be authenticated");
        }

        private void validateInstaClient()
        {
            if (instaClient == null)
                throw new ArgumentException("Insta Client is null");
        }

        private void validateStockClient()
        {
            if (stockClient == null)
                throw new ArgumentException("Stock Client is null");
        }

        #endregion
    }
}