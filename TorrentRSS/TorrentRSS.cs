using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace TorrentRSS
{
    class TorrentRSS
    {
        static void Main(string[] args)
        {
            GetContents("torrentwiz", "me", 38, "drama", 1);
            //GetContents("torrentview", "com", 48, "drama", 1);

            //GetContents("torrentlee", "me", 28, "mov", 1);
            //GetContents("torrentdia", "com", 100, "torrent_drama", 1);
        }

        static async Task GetContents(string site, string tld, int count, string board, int page)
        {
            TorrentRSS torrentRss = new TorrentRSS();
            string[] contents = new string[] {"", ""};
            string domain = torrentRss.GetDomain(site, tld, count);
            string html = torrentRss.GetHtml(domain + "/bbs/board.php?bo_table=" + board + "&page=" + page).Trim();
            Regex urlRegex = new Regex("<div class=\"wr-subject\">\n<a href=\"https://(.+)\" class=\"item-subject\">");
            Regex subjectRegex = new Regex("<h1 class=\"panel-title\">\n(.+) </h1>");
            Match subjectMatch;
            Regex magnetRegex = new Regex("<a href=\"magnet:.xt=urn:btih:(.+)\"");
            Match magnetMatch;
            MatchCollection matchCollection = urlRegex.Matches(html);
            foreach (Match match in matchCollection)
            {
                string url = match.Value;
                url = Regex.Replace(url, "\" class=\"item-subject\">", "");
                url = Regex.Replace(url, "amp;", "");
                url = Regex.Replace(url, "<div class=\"wr-subject\">\n<a href=\"", "");
                url = Regex.Replace(url, "&page(.+)", "");

                string contentHtml = torrentRss.GetHtml(url).Trim();
                subjectMatch = subjectRegex.Match(contentHtml);
                magnetMatch = magnetRegex.Match(contentHtml);

                string subject = subjectMatch.Value;
                subject = Regex.Replace(subject, "<h1 class=\"panel-title\">\n", "");
                subject = Regex.Replace(subject, " </h1>", "");
                Console.WriteLine(subject);

                string magnet = magnetMatch.Value;
                magnet = Regex.Replace(magnet, "<a href=\"", "");
                magnet = Regex.Replace(magnet, "\" target=\"_self\"", "");
                Console.WriteLine(magnet);

                Console.WriteLine(url);
            }
        }

        string GetDomain(string domain, string tld, int count)
        {
            while (true)
            {
                try
                {
                    HttpWebRequest request =
                        (HttpWebRequest) WebRequest.Create("http://www." + domain + count + "." + tld);
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //Console.WriteLine("http://www." + domain + count + "." + tld);
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

        string GetHtml(string Url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(Url);
            httpWebRequest.Method = "GET";
            WebResponse webResponse = httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string html = streamReader.ReadToEnd();
            streamReader.Close();
            webResponse.Close();
            return html;
        }
    }
}