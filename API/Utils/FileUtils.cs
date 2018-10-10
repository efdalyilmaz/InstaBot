using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Unsplasharp.Models;

namespace InstaBot.API.Utils
{
    public class FileUtils
    {

        public static List<long> ReadRequestedList()
        {
            List<long> requestedList = new List<long>();
            try
            {
                string filePath =  System.Configuration.ConfigurationManager.AppSettings.Get("RequestedFilePath");
                if (!File.Exists(filePath))
                {
                    return requestedList;
                }

                string[] lines = File.ReadAllLines(filePath);
                requestedList = lines.Select(Int64.Parse).ToList();

                return requestedList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetFullDirectory(string directory, string searchCategory)
        {
            return Path.Combine(directory, searchCategory);
        }

        public static string GetFullFilePath(string directory, string fileName, string extension)
        {
            string filePath = Path.Combine(directory, fileName);
            return Path.ChangeExtension(filePath, extension);
        }
    }
}

