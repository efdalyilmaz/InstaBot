using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.API.Models
{
    public static class LocationExtensions
    {
        public static Location ToLocation(this Unsplasharp.Models.Location location)
        {
            return new Location
            {
                Title = location.Title,
                Name = location.Name,
                Country = location.Country,
                City = location.City
            };
        }
    }
}
