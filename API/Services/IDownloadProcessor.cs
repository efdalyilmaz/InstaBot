﻿using InstaBot.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


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