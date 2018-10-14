
using InstaBot.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unsplasharp;
using Unsplasharp.Models;

namespace InstaBot.API.Services
{
    public class StockService : IStockService
    {
        private readonly UnsplasharpClient stockClient;
        
        public StockService(string applicationId, string secretKey)
        {
            stockClient = new UnsplasharpClient(applicationId, secretKey);
        }

        public async Task<List<InstaBot.API.Models.Photo>> SearchPhotosAsync(string stockCategoryName, int page= ApiConstans.PHOTO_SEARCH_DEFAULT_PAGE, int perPage = ApiConstans.PHOTO_SEARCH_DEFAULT_PER_PAGE)
        {
            List<Unsplasharp.Models.Photo> searchList = await stockClient.SearchPhotos(stockCategoryName, page, perPage);
            return searchList.ToPhotoList();
        }

        public async Task<List<InstaBot.API.Models.Photo>> SearchNewPhotosAsync(string stockCategoryName, int photoCount, List<string> downloadedPhotos)
        {
            List<InstaBot.API.Models.Photo> photoList = new List<InstaBot.API.Models.Photo>();
            int page = ApiConstans.PHOTO_SEARCH_DEFAULT_PAGE;
            while (photoList.Count == 0)
            {
                photoList = await this.SearchPhotosAsync(stockCategoryName, page);
                photoList.RemoveAll(p => downloadedPhotos.Contains(p.Id));
                page++;
            }

            photoList = photoList.Take(photoCount).ToList();
            for (int i = 0; i < photoList.Count; i++)
            {
                //for missing information
                photoList[i] = (await stockClient.GetPhoto(photoList[i].Id)).ToPhoto();
            }

            return photoList;
        }
    }
}
