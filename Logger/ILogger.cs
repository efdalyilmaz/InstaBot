using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.Logger
{
    public interface ILogger
    {
        void Write(string text);

        void WriteAllProperties(object obj);
    }
}
