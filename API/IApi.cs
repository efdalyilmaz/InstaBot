using InstaBot.API.Filter;
using InstaBot.API.Processors;
using InstaSharper.Classes.Models;
using System.Threading.Tasks;

namespace InstaBot.API
{
    public interface IApi
    {
        Task Login();

        Task Logout();

        Task MakeFollowRequestAsync(string userName, IFilter<InstaUserShort> filter=null);
        
        Task UploadPhotoAsync(string stockCategoryName, int photoCount, IDownloadProcessor downloadProcessor);
    }
}