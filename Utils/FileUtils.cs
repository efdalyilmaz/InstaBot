using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Unsplasharp.Models;

namespace InstaBot.Utils
{
    public class FileUtils
    {
        public static List<Photo> ListOfDownloadedPhoto { get; set; }

        public static void DownloadAllPhotos(List<Photo> photoList)
        {
            foreach (var photo in photoList)
            {
                DownloadPhoto(photo);
            }
        }

        public static void DownloadPhoto(Photo photo)
        {
            try
            {
                string url = "https://unsplash.com/photos/" + photo.Id + "/download?force=true";
                const string imagesSubdirectory = @"D:\unsplash\holiday";
                string filePath = imagesSubdirectory + @"\" + photo.Id + ".jpg";
                if (File.Exists(filePath))
                {
                    return;
                }
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(url), filePath);
                }

                ListOfDownloadedPhoto.Add(photo);
            }
            catch (Exception ex)
            {
            }
        }
    }
}

