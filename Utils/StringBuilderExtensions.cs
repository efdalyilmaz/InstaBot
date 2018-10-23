using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBot.Utils
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AddTag(this StringBuilder tags, string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return tags;
            }

            string replaced = text.Replace(" ", "");
            if (!tags.ToString().Contains(replaced))
            {
                tags.Append($"#{replaced} ");
            }

            return tags;
        }
    }
}
