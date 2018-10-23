using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.Models
{
    public class Info
    {
        public bool Succeeded { get; private set; }
        public string Message { get; }

        public Info()
        {
            Succeeded = true;
            Message = String.Empty;
        }

        public Info(bool succeed, string message)
        {
            this.Succeeded = succeed;
            this.Message = message;
        }
    }
}
