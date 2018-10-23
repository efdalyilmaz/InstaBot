using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.Models
{
    public class Media
    {
        public string Id { get; set; }

        public int LikesCount { get; set; }

        public UserInfo User { get; set; }
    }
}
