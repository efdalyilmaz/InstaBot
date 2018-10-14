using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.API.Models
{
    public class Photo
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string DownloadLink { get; set; }

        public Location Location { get; set; }

        public UserInfo User { get; set; }

        public string GetCaption()
        {
            StringBuilder caption = new StringBuilder();

            if (!String.IsNullOrEmpty(this.Description))
            {
                caption.Append(this.Description);
                caption.Append(System.Environment.NewLine);
            }


            StringBuilder locationTags = new StringBuilder();
            if (this.Location != null)
            {
                if (!String.IsNullOrEmpty(this.Location.Title))
                {
                    caption.Append(this.Location.Title);
                    caption.Append(System.Environment.NewLine);
                }

                if (!String.IsNullOrEmpty(this.Location.Country))
                {
                    locationTags.Append($"#{this.Location.Country.Replace(" ", "")} ");
                }

                if (!String.IsNullOrEmpty(this.Location.City))
                {
                    locationTags.Append($"#{this.Location.City.Replace(" ", "")} ");
                }

                if (!String.IsNullOrEmpty(this.Location.Name) && !locationTags.ToString().Contains(this.Location.Name))
                {
                    locationTags.Append($"#{this.Location.Name.Replace(" ", "")} ");
                }
            }

            if (!String.IsNullOrEmpty(this.User.Name))
            {
                caption.Append($"Thanx to {this.User.Name}");
                caption.Append(System.Environment.NewLine);
            }


            caption.Append($"#holiday #travel #trip #explore #discover {locationTags.ToString().ToLower(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))}");

            return caption.ToString();
        }
    }
}
