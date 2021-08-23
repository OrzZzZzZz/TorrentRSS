using System;
using System.Net;

namespace TorrentRSS
{
    class TorrentRSS
    {
        static void Main(string[] args)
        {
            TorrentRSS torrentRss = new TorrentRSS();
            string domain = torrentRss.GetDomain("torrentview");
            Console.WriteLine(domain);
        }

        string GetHtml(string empty)
        {
            return "";
        }

        string GetDomain(string domain)
        {
            int i = 100;
            while (true)
            {
                Console.WriteLine("http://www." + domain + i + ".com");
                try
                {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://www." + domain + i + ".com");
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return "OK";
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    continue;
                }
                finally
                {
                    i--;
                }
            }
        }
    }
}