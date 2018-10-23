using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Write(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteAllProperties(object obj)
        {
            if(obj == null)
            {
                return;
            }

            string sObj = JsonConvert.SerializeObject(obj);
            JObject parsed = JObject.Parse(sObj);
            foreach (var pair in parsed)
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }

        }
    }
}
