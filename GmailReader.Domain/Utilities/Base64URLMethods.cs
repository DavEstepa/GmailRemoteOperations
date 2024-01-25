using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailReader.Domain.Utilities
{
    public static class Base64URLHelpers
    {
        public static string Base64UrlDecode(string base64UrlSafeString)
        {
            // Replace '-' with '+' and '_' with '/'
            base64UrlSafeString = base64UrlSafeString.Replace('-', '+').Replace('_', '/');

            // Pad with '=' characters until the length is a multiple of 4
            int padLength = (4 - base64UrlSafeString.Length % 4) % 4;
            base64UrlSafeString = base64UrlSafeString.PadRight(base64UrlSafeString.Length + padLength, '=');

            // Decode the Base64 URL-safe encoded string
            byte[] base64Bytes = Convert.FromBase64String(base64UrlSafeString);
            return System.Text.Encoding.UTF8.GetString(base64Bytes);
        }
    }
}
