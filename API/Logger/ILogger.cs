using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.API.Logger
{
    public interface ILogger
    {
        void Write(string text);
    }
}
