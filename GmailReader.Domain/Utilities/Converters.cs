using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailReader.Domain.Utilities
{
    public static class Converters
    {
        public static byte[] ConvertStringToByteArray(this string inputString)
        {
            // Use UTF-8 encoding, you can change it to other encodings as needed
            Encoding encoding = Encoding.UTF8;

            // Convert the string to byte array
            byte[] byteArray = encoding.GetBytes(inputString);

            return byteArray;
        }

        public static string ConvertByteArrayToString(this byte[] byteArray)
        {
            Encoding encoding = Encoding.UTF8;
            string resultString = encoding.GetString(byteArray);
            return resultString;
        }
    }
}
