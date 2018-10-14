using InstaBot.API.Logger;
using InstaBot.API.Models;
using InstaBot.API.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace InstaBot.API.Processors
{
    public class DownloadProcessor : IDownloadProcessor
    {
        private readonly ILogger logger;
        public string Directory { get; private set; }

        public DownloadProcessor(ILogger logger, string directory)
        {
            this.logger = logger;
            this.Directory = directory;
        }

        public List<string> GetAllDownloadedPhotoNames()
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(Directory);
                FileInfo[] files = d.GetFiles("*." + ApiConstans.PHOTO_EXTENSION);

                return files.Select(f => Path.GetFileNameWithoutExtension(f.Name)).ToList<string>();

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
                client.DownloadFileTaskAsync(new Uri(uri), FileUtils.GetFullFilePath(Directory, photo.Id, ApiConstans.PHOTO_EXTENSION));
            }
        }

        public void DownloadPhoto(Photo photo)
        {
            using (WebClient client = new WebClient())
            {
                string uri = photo.DownloadLink;
                client.DownloadFile(new Uri(uri), FileUtils.GetFullFilePath(Directory, photo.Id, ApiConstans.PHOTO_EXTENSION));
            }
        }

    }
}
