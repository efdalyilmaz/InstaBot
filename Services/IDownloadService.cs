using InstaBot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace InstaBot.Services
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
