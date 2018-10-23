using InstaBot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InstaBot.Utils
{
    public class FileUtils
    {

        public static List<long> ReadRequestedList()
        {
            List<long> requestedList = new List<long>();
            try
            {
                string filePath = System.Configuration.ConfigurationManager.AppSettings.Get("RequestedFilePath");
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

        public static void WriteAllToRequestedFile(List<UserInfo> privateUserList)
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings.Get("RequestedFilePath");
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            using (StreamWriter w = File.AppendText(filePath))
            {
                foreach (var item in privateUserList)
                {
                    w.WriteLine(item.Id.ToString());
                }

            }

        }

        public static List<string> GetDirectorysFileNames(string directory, string searchPattern = ApiConstans.PHOTO_EXTENSION_SEARCH_PATTERN)
        {
            DirectoryInfo d = new DirectoryInfo(directory);
            FileInfo[] files = d.GetFiles(searchPattern, SearchOption.AllDirectories);

            return files.Select(f => Path.GetFileNameWithoutExtension(f.Name)).ToList<string>();
        }

        public static void WriteAllListToFile<T>(string filePath, List<T> list)
        {
            using (StreamWriter w = new StreamWriter(filePath, true))
            {
                foreach (var item in list)
                {
                    w.WriteLine(item.ToString());
                }
            }
        }

        public static List<T> ReadFile<T>(string filePath)
        {
            List<T> list = new List<T>();
            try
            {
                if (!File.Exists(filePath))
                {
                    return list;
                }

                string[] lines = File.ReadAllLines(filePath);
                list = lines.Cast<T>().ToList();

                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

