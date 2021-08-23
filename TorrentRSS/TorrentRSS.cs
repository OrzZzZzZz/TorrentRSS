using System;
using System.IO;
using System.Net;

namespace TorrentRSS
{
    class TorrentRSS
    {
        static void Main(string[] args)
        {
            GetContents("torrentview", "drama", 1);
        }

        static void GetContents(string site, string board, int page)
        {
            TorrentRSS torrentRss = new TorrentRSS();
            string domain;
            string html = null;
            switch (site)
            {
                case "torrentview":
                    domain = torrentRss.GetDomain("torrentview", "com");
                    html = torrentRss.GetHtml(domain);
                    break;
                case "torrentsee":
                    domain = torrentRss.GetDomain("torrentsee", "com");
                    html = torrentRss.GetHtml(domain);
                    break;
                case "torrentlee":
                    domain = torrentRss.GetDomain("torrentlee", "me");
                    html = torrentRss.GetHtml(domain);
                    break;
                case "torrentwiz":
                    domain = torrentRss.GetDomain("torrentwiz", "me");
                    html = torrentRss.GetHtml(domain);
                    break;
                case "torrentdia":
                    domain = torrentRss.GetDomain("torrentdia", "com");
                    html = torrentRss.GetHtml(domain);
                    break;
            }

            Console.WriteLine(html);
        }


        string GetHtml(string Url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(Url);
            httpWebRequest.Method = "GET";
            WebResponse webResponse = httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            webResponse.Close();
            Console.WriteLine(result);
            return result;
        }

        string GetDomain(string domain, string tld) //tld = top level domain
        {
            int count = 200;
            while (true)
            {
                //Console.WriteLine("http://www." + domain + i + "."+tld);
                try
                {
                    HttpWebRequest request =
                        (HttpWebRequest) WebRequest.Create("http://www." + domain + count + "." + tld);
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("http://www." + domain + count + "." + tld);
                        return "http://www." + domain + count + "." + tld;
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
                finally
                {
                    count--;
                }
            }
        }
    }
}