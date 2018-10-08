using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace instabot
{
    public class Program
    {

        #region Privates
        private static string userName = "";
        private static string password = "";
        #endregion Privates

        static void Main(string[] args)
        {
            getUserInfo();

            Console.Write("Whose followers: ");
            string whosefollowers = Console.ReadLine();

            using (Bot bot = new Bot(userName, password))
            {
                Task.WaitAny(bot.MakeFollowRequestToPrivateAccount(whosefollowers));
            }

            Console.Read();
        }

        private static void getUserInfo()
        {
            Console.Write("UserName : ");
            userName = Console.ReadLine();

            Console.Write("Password : ");
            password = readPassword();
            Console.WriteLine("\n--------------");

        }

        private static string readPassword()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            return pass;
        }
    }
}
