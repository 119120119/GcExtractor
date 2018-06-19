using System;
using GcExtractor.Service;

namespace GcExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
           var gmail = new GmailCusService();
            foreach (var label in gmail.GetLabels())
            {
                Console.WriteLine(label);
            }
        }
    }
}
