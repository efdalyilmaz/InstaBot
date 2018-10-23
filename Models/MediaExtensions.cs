using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.Models
{
    public static class MediaExtensions
    {
        public static Media ToMedia(this InstaMedia instaMedia)
        {
            return new Media
            {
                Id = instaMedia.Pk,
                LikesCount = instaMedia.LikesCount,
                User = instaMedia.User.ToUserInfo()
            };
        }

        public static List<Media> ToMediaList(this List<InstaMedia> instaMediaList)
        {
            List<Media> mediaList = new List<Media>();
            foreach (var item in instaMediaList)
            {
                mediaList.Add(item.ToMedia());
            }

            return mediaList;
        }
    }
}
