using InstaBot.Filter;
using InstaBot.Models;
using InstaBot.Services;
using System.Threading.Tasks;

namespace InstaBot
{
    public interface IApi
    {
        Task LikeMediaAsync(string hashtag);

        Task MakeFollowRequestAsync(string userName, IFilter<UserInfo> filter=null);

        Task MakeAllFollowingsFollowersFollowRequestAsync(int top=10, IFilter<UserInfo> filter = null);

        Task UploadPhotoAsync(string stockCategoryName, int photoCount, IDownloadService downloadProcessor);
    }
}