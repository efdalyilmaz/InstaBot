using InstaBot.Models;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Services
{
    public interface IInstaService
    {
        Task Login();

        Task Logout();

        Task LikeMediaAsync(string mediaId);

        Task<UserInfo> GetCurrentUserInfo();

        Task<List<Media>> GetTagFeedAsync(string hashtag, int maxPageToLoad = 10);

        Task<List<UserInfo>> GetUserFollowers(string userName, int maxPageToLoad = 10);

        Task<List<UserInfo>> GetCurrentUserFollowings(int maxPageToLoad = 10);

        Task FollowUserAsync(long userId);

        Task UploadPhotoAsync(string fullpath, string caption);
        
    }
}
