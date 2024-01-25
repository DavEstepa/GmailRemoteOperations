using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GmailReader.Domain.Extensions
{
    public static class EmailExtractor
    {
        public static string ExtractEmailInChars(this string completeString, char inferior, char superior )
        {
            string pattern = $@"{Regex.Escape(inferior.ToString())}([^>{Regex.Escape(superior.ToString())}]*){Regex.Escape(superior.ToString())}";

            MatchCollection matches = Regex.Matches(completeString, pattern);
            var returnedValue = new List<string>();
            foreach (Match match in matches)
            {
                string extractedString = match.Groups[1].Value;
                returnedValue.Add(extractedString);
            }
            if (returnedValue.Count == 0) return "";
            return returnedValue.First();
        }
    }
}
