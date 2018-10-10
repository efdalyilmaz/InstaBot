using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.Utils
{
    public class ConsoleUtils
    {
        public static Tuple<string,string> GetUserInfo()
        {
            Console.Write("UserName : ");
            string userName = Console.ReadLine();

            Console.Write("Password : ");
            string  password = ReadPassword();
            Console.WriteLine("\n----------------------------");

            return new Tuple<string, string>(userName, password);
        }

        public static string ReadPassword()
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
