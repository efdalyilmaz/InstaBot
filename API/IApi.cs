using InstaBot.API.Filter;
using InstaBot.API.Models;
using InstaBot.API.Processors;
using System.Threading.Tasks;

namespace InstaBot.API
{
    public interface IApi
    {
        Task LikeMediaAsync(string hashtag);

        Task MakeFollowRequestAsync(string userName, IFilter<UserInfo> filter=null);

        Task MakeAllFollowingsFollowersFollowRequestAsync(int top=10, IFilter<UserInfo> filter = null);

        Task UploadPhotoAsync(string stockCategoryName, int photoCount, IDownloadService downloadProcessor);
    }
}