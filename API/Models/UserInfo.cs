using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.API.Models
{
    public class UserInfo
    {
        public long Id { get; set; }

        public bool IsPrivate { get; set; }
     
        public bool HasProfilePicture { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }
    }
}
