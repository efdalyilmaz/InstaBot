using InstaBot.Logger;
using InstaBot.Models;
using InstaBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace InstaBot.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly ILogger logger;
        public string Directory { get; private set; }
        public string Category { get; private set; }
        public string FullDirectory { get; private set; }


        public DownloadService(ILogger logger, string directory, string category)
        {
            this.logger = logger;
            this.Directory = directory;
            this.Category = category;
            this.FullDirectory = FileUtils.GetFullDirectory(Directory, Category);

            if (!System.IO.Directory.Exists(FullDirectory))
            {
                System.IO.Directory.CreateDirectory(FullDirectory);
            }
        }

        public List<string> GetAllDownloadedPhotoNames()
        {
            try
            {
                string filePath = FileUtils.GetFullFilePath(Directory, ApiConstans.DOWNLOADS_FILE_NAME, ApiConstans.FILE_EXTENSION);
                return FileUtils.ReadFile<string>(filePath);
            }
            catch (Exception ex)
            {
                logger.Write(ex.ToString());
                throw;
            }
        }

        public void WriteDownloadedPhotoNames(List<Photo> list)
        {
            try
            {
                string filePath = FileUtils.GetFullFilePath(Directory, ApiConstans.DOWNLOADS_FILE_NAME, ApiConstans.FILE_EXTENSION);
                List<string> names = list.Select(p => p.Id).ToList();

                FileUtils.WriteAllListToFile<string>(filePath, names);
            }
            catch (Exception ex)
            {
                logger.Write(ex.ToString());
                throw;
            }
        }

        public async Task DownloadAllPhotosAsync(List<Photo> photoList)
        {
            await Task.Run(() => Parallel.ForEach(photoList, photo =>
            {
                DownloadPhoto(photo);
            }));
        }

        public async Task DownloadPhotoAsync(Photo photo)
        {
            using (WebClient client = new WebClient())
            {
                string uri = photo.DownloadLink;
                client.DownloadFileTaskAsync(new Uri(uri), FileUtils.GetFullFilePath(FullDirectory, photo.Id, ApiConstans.PHOTO_EXTENSION));
            }
        }

        public void DownloadPhoto(Photo photo)
        {
            using (WebClient client = new WebClient())
            {
                string uri = photo.DownloadLink;
                client.DownloadFile(new Uri(uri), FileUtils.GetFullFilePath(FullDirectory, photo.Id, ApiConstans.PHOTO_EXTENSION));
            }
        }

    }
}
