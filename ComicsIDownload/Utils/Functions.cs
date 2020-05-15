using System;

namespace ComicsIDownload.Utils
{
    static class Functions
    {
        public static string FormatSize(long bytes)
        {
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
                number /= 1024;
            return string.Format("{0:n2}", number);
        }
    }
}
