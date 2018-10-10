using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.API.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Write(string text)
        {
            Console.Write(text);
        }
    }
}
