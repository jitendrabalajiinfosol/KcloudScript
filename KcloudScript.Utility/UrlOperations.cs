using System;
using System.Collections.Generic;
using System.Linq;

namespace KcloudScript.Utility
{
    public static class UrlOperations
    {
        private static List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
        private static List<char> characters = new List<char>()
    {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
    'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B',
    'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
    'Q', 'R', 'S',  'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', '_'};


        private static string GetRandomString()
        {
            string url = string.Empty;
            Enumerable.Range(64, 122)
              .Where(i => i < 123 || i > 64)
              .OrderBy(o => new Random().Next())
              .ToList()
              .ForEach(i => url += Convert.ToChar(i));
            string token = url.Substring(new Random().Next(0, url.Length), new Random().Next(2, 6));

            return token;
        }

        public static string GenerateUrl()
        {
            string Url = "";
            Random rand = new Random();
            for (int i = 0; i < 9; i++)
            {
                int random = rand.Next(0, 3);
                if (random == 1)
                {
                    random = rand.Next(0, numbers.Count);
                    Url += numbers[random].ToString();
                }
                else
                {
                    random = rand.Next(0, characters.Count);
                    Url += characters[random].ToString();
                }
            }
            return "https://" + Url + ".com/" + "?rdata=" + GetRandomString();
        }
    }
}
