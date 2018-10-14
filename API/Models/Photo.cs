using InstaBot.API.Utils;
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

                locationTags.AddTag(this.Location.Country)
                            .AddTag(this.Location.City)
                            .AddTag(this.Location.Name);
            }

            if (!String.IsNullOrEmpty(this.User.Name))
            {
                caption.Append($"Thanx to {this.User.Name}");
                caption.Append(System.Environment.NewLine);
            }

            string additionalLocationTags = locationTags.ToString().ToLower(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            caption.Append($"#holiday #travel #trip #explore #discover {additionalLocationTags}");

            return caption.ToString();
        }
    }
}
