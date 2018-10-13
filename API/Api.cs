using InstaBot.API.Builder;
using InstaBot.API.Filter;
using InstaBot.API.Logger;
using InstaBot.API.Processors;
using InstaBot.API.Utils;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                    .SetRequestDelay(RequestDelay.FromSeconds(1, 3))
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

        public async Task LikeMediaAsync(string hashtag)
        {
            validateInstaClient();
            validateLoggedIn();

            IResult<InstaTagFeed> tagFeeds = await instaClient.GetTagFeedAsync(hashtag, PaginationParameters.MaxPagesToLoad(10));
            List<InstaMedia> mediaList = tagFeeds.Value.Medias.FindAll(m => m.LikesCount > ApiConstans.MIN_LIKES_COUNT).Take(ApiConstans.MAX_REQUEST_COUNT).ToList();

            for (int i = 0; i < mediaList.Count; i++)
            {
                instaClient.LikeMediaAsync(mediaList[i].Pk);
                await Task.Delay(ApiConstans.DELAY_TIME);
                logger.Write($"Liked Media User: {mediaList[i].User.UserName}, Remaining Media {mediaList.Count - i - 1}");
            }

        }

        public async Task MakeFollowRequestAsync(string userName, IFilter<InstaUserShort> filter = null)
        {
            validateInstaClient();
            validateLoggedIn();

            IResult<InstaUserShortList> userShortList = await instaClient.GetUserFollowersAsync(userName, PaginationParameters.MaxPagesToLoad(20));
            filter = filter ?? FollowerFilter.DefaultFilter();
            var filtered = filter.Apply(userShortList.Value);

            for (int i = 0; i < filtered.Count; i++)
            {
                instaClient.FollowUserAsync(filtered[i].Pk);
                await Task.Delay(ApiConstans.DELAY_TIME);
                logger.Write($"Requested UserName : {filtered[i].UserName}, Remaining User {filtered.Count - i - 1}");
            }

            FileUtils.WriteAllToRequestedFile(filtered);
        }

        public async Task UploadPhotoAsync(string stockCategoryName,int photoCount, IDownloadProcessor downloadProcessor)
        {
            validateInstaClient();
            validateLoggedIn();
            validateStockClient();
            
            List<Photo> photoList = await getDownloadablePhotoList(stockCategoryName, photoCount, downloadProcessor);
           
            await downloadProcessor.DownloadAllPhotosAsync(photoList);


            int uploadedPhoto = 1;
            logger.Write(String.Format("Downloaded photo count {0}", photoList.Count));
            foreach (var photo in photoList)
            {
                string filePath = FileUtils.GetFullFilePath(downloadProcessor.Directory, photo.Id, ApiConstans.PHOTO_EXTENSION);
                string caption = createCaptionText(photo);

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

        private async Task<List<Photo>> getDownloadablePhotoList(string stockCategoryName, int photoCount, IDownloadProcessor downloadProcessor)
        {
            var downloaded = downloadProcessor.GetAllDownloadedPhotoNames();
            
            List<Photo> photoList = new List<Photo>();
            int page = 6;
            while(photoList.Count == 0)
            {
                List<Photo> searchList = await stockClient.SearchPhotos(stockCategoryName, page, 30);
                photoList = searchList;
                photoList.RemoveAll(p => downloaded.Contains(p.Id));
                page++;
            }

            photoList = photoList.Take(photoCount).ToList();
            for (int i = 0; i < photoList.Count; i++)
            {
                //for missing information
                photoList[i] = await stockClient.GetPhoto(photoList[i].Id);
            }

            return photoList;
        }

        private string createCaptionText(Photo photo)
        {
            StringBuilder caption = new StringBuilder();

            if (!String.IsNullOrEmpty(photo.Description))
            {
                caption.Append(photo.Description);
                caption.Append(System.Environment.NewLine);
            }
            

            StringBuilder locationTags = new StringBuilder();
            if (photo.Location != null)
            {
                if (!String.IsNullOrEmpty(photo.Location.Title))
                {
                    caption.Append(photo.Location.Title);
                    caption.Append(System.Environment.NewLine);
                }
                
                if (!String.IsNullOrEmpty(photo.Location.Country))
                {
                    locationTags.Append($"#{photo.Location.Country.ToLower().Replace(" ", "")} ");
                }

                if (!String.IsNullOrEmpty(photo.Location.City))
                {
                    locationTags.Append($"#{photo.Location.City.ToLower().Replace(" ", "")} ");
                }

                if (!String.IsNullOrEmpty(photo.Location.Name) && !locationTags.ToString().Contains(photo.Location.Name))
                {
                    locationTags.Append($"#{photo.Location.Name.ToLower().Replace(" ", "")} ");
                }
            }

            if (!String.IsNullOrEmpty(photo.User.Name))
            {
                caption.Append($"Thanx to {photo.User.Name}");
                caption.Append(System.Environment.NewLine);
            }
            
            
            caption.Append($"#holiday #travel #trip #explore #discover {locationTags.ToString()}");
            

            return caption.ToString();
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