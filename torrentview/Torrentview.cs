using System;
using System.Net;

namespace torrentview
{
    class Torrentview
    {
        static void Main(string[] args)
        {
            Torrentview torrent = new Torrentview();
            string domain = torrent.GetDomain("torrentview");
            Console.WriteLine(domain);
        }

        private string GetHtml(string empty)
        {
            return "";
        }

        private string GetDomain(string domain)
        {
            var num = 35;
            for (var i = num; i < 40; i++)
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://www." + domain + i + ".com");
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                HttpStatusCode status = response.StatusCode;
                Console.WriteLine(status);
            }

            //domain = domain + "35";
            return domain;
        }
    }
}