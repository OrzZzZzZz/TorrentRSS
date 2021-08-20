using System;

namespace torrentview
{
    class Torrentview
    {
        private string domain;
        private string category;

        static void Main(string[] args)
        {
            Torrentview torrentview = new Torrentview();
            torrentview.domain = GetDomain();
        }

        private static string GetDomain()
        {
            return "";
        }
    }
}