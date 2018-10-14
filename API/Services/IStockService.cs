using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.API.Services
{
    public interface IStockService
    {
        Task<List<InstaBot.API.Models.Photo>> SearchPhotosAsync(string stockCategoryName, int page = ApiConstans.PHOTO_SEARCH_DEFAULT_PAGE, int perPage = ApiConstans.PHOTO_SEARCH_DEFAULT_PER_PAGE);

        Task<List<InstaBot.API.Models.Photo>> SearchNewPhotosAsync(string stockCategoryName, int photoCount, List<string> downloadedPhotos);
    }
}
