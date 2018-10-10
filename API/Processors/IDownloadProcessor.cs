using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unsplasharp.Models;

namespace InstaBot.API.Processors
{
    public interface IDownloadProcessor
    {
        string Directory { get; }

        List<string> GetAllDownloadedPhotoNames();

        Task DownloadAllPhotosAsync(List<Photo> photoList);

        Task DownloadPhotoAsync(Photo photo);

        void DownloadPhoto(Photo photo);
    }
}
