using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Classes.ResponseWrappers;
using InstaSharper.Classes.ResponseWrappers.BaseResponse;

namespace InstaBot.API
{
    public interface IApi
    {
        Task Login();

        Task Logout();

        Task MakeFollowRequestAsync(string userName);
        
        Task UploadPhotoAsync(string stockCategoryName);
    }
}