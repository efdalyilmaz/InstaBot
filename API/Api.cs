using InstaBot.API.Filter;
using InstaBot.API.Logger;
using InstaBot.API.Models;
using InstaBot.API.Processors;
using InstaBot.API.Services;
using InstaBot.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBot.API
{
    internal class Api : IApi
    {
        private readonly IInstaService instaService;
        private readonly IStockService stockService;
        private readonly ILogger logger;

        public Api(ILogger logger, string userName, string password)
        {
            this.logger = logger;
            instaService = new InstaService(logger, userName, password);
        }

        public Api(ILogger logger, string userName, string password, string applicationId, string secretKey) : this(logger, userName, password)
        {
            stockService = new StockService(applicationId, secretKey);
        }

        public async Task LikeMediaAsync(string hashtag)
        {
            List<Media> mediaList = await instaService.GetTagFeedAsync(hashtag);
            mediaList = mediaList.FindAll(m => m.LikesCount > ApiConstans.MIN_LIKES_COUNT).Take(ApiConstans.MAX_REQUEST_COUNT).ToList();

            for (int i = 0; i < mediaList.Count; i++)
            {
                instaService.LikeMediaAsync(mediaList[i].Id);
                await Task.Delay(ApiConstans.DELAY_TIME);
                logger.Write($"Liked Media User: {mediaList[i].User.UserName}, Remaining Media {mediaList.Count - i - 1}");
            }

        }

        public async Task MakeFollowRequestAsync(string userName, IFilter<UserInfo> filter = null)
        {
            List<UserInfo> userInfoList = await instaService.GetUserFollowers(userName,20);
            filter = filter ?? FollowerFilter.DefaultFilter();

            var filtered = filter.Apply(userInfoList);

            for (int i = 0; i < filtered.Count; i++)
            {
                instaService.FollowUserAsync(filtered[i].Id);
                await Task.Delay(ApiConstans.DELAY_TIME);
                logger.Write($"Requested UserName : {filtered[i].UserName}, Remaining User {filtered.Count - i - 1}");
            }

            FileUtils.WriteAllToRequestedFile(filtered);
        }

        public async Task UploadPhotoAsync(string stockCategoryName,int photoCount, IDownloadProcessor downloadProcessor)
        {
            List<string> downloadedPhotos = downloadProcessor.GetAllDownloadedPhotoNames();
            List<Photo> photoList = await stockService.SearchNewPhotosAsync(stockCategoryName, photoCount, downloadedPhotos);
            await downloadProcessor.DownloadAllPhotosAsync(photoList);

            int uploadedPhoto = 1;
            logger.Write(String.Format("Downloaded photo count {0}", photoList.Count));
            foreach (var photo in photoList)
            {
                string filePath = FileUtils.GetFullFilePath(downloadProcessor.Directory, photo.Id, ApiConstans.PHOTO_EXTENSION);
                
                await instaService.UploadPhotoAsync(filePath, photo.GetCaption());
                
                logger.Write(String.Format("{0}. uploaded. PhotoId : {1} ", uploadedPhoto, photo.Id));
                uploadedPhoto++;

            }
        }
    }
}