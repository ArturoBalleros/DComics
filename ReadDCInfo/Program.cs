using ReadDCInfo.Services;
using System;

namespace ReadDCInfo
{
    static class Program
    {
        static void Main(string[] args)
        {
            ScrapService scrapService = new ScrapService();
            scrapService.scrapReadDC();
            Console.WriteLine("Hello World!");
        }
    }
}
