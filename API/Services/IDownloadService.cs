using InstaBot.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace InstaBot.API.Processors
{
    public interface IDownloadService
    {
        string Directory { get; }

        string Category { get; }

        string FullDirectory { get; }

        List<string> GetAllDownloadedPhotoNames();

        void WriteDownloadedPhotoNames(List<Photo> list);

        Task DownloadAllPhotosAsync(List<Photo> photoList);

        Task DownloadPhotoAsync(Photo photo);

        void DownloadPhoto(Photo photo);
    }
}
