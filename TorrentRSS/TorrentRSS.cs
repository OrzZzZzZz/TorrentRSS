using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.Xml;

namespace TorrentRSS
{
    class TorrentRss
    {
        static void Main(string[] args)
        {
            Thread torrentwiz = new Thread(() => GetContents("torrentwiz", "me", 38, "drama", 1));
            Thread torrentlee = new Thread(() => GetContents("torrentlee", "me", 28, "drama", 1));
            Thread torrentview = new Thread(() => GetContents("torrentview", "com", 48, "drama", 1));
            torrentwiz.Start();
            // torrentlee.Start();
            // torrentview.Start();
        }

        static void GetContents(string site, string tld, int count, string board, int page)
        {
            TorrentRss torrentRss = new TorrentRss();
            string domain = torrentRss.GetDomain(site, tld, count);
            string html = torrentRss.GetHtml(domain + "/bbs/board.php?bo_table=" + board + "&page=" + page).Trim();
            Regex urlRegex = new Regex("<div class=\"wr-subject\">\n<a href=\"https://(.+)\" class=\"item-subject\">");
            Regex subjectRegex = new Regex("<h1 class=\"panel-title\">\n(.+) </h1>");
            Regex magnetRegex = new Regex("<a href=\"magnet:.xt=urn:btih:(.+)\"");
            MatchCollection urlCollection = urlRegex.Matches(html);
            CreateXML();
            string url = null;
            string subject = null;
            string magnet = null;
            foreach (Match urlMatch in urlCollection)
            {
                url = urlMatch.Value;
                url = Regex.Replace(url, "\" class=\"item-subject\">", "");
                url = Regex.Replace(url, "amp;", "");
                url = Regex.Replace(url, "<div class=\"wr-subject\">\n<a href=\"", "");
                url = Regex.Replace(url, "&page(.+)", "");

                string contentHtml = torrentRss.GetHtml(url).Trim();
                var subjectMatch = subjectRegex.Match(contentHtml);
                var magnetMatch = magnetRegex.Match(contentHtml);

                subject = subjectMatch.Value;
                subject = Regex.Replace(subject, "<h1 class=\"panel-title\">\n", "");
                subject = Regex.Replace(subject, " </h1>", "");
                subject = Regex.Replace(subject, ".mp4", "");

                magnet = magnetMatch.Value;
                magnet = Regex.Replace(magnet, "<a href=\"", "");
                magnet = Regex.Replace(magnet, "\" target=\"_self\"", "");

                Console.WriteLine(subject);
                Console.WriteLine(magnet);
                Console.WriteLine(url);

                AddXML(subject, magnet, url);
            }
        }

        private static void CreateXML()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode rss = xmlDocument.CreateElement("rss");
            xmlDocument.AppendChild(rss);
            XmlNode channel = xmlDocument.CreateElement("channel");
            rss.AppendChild(channel);
            xmlDocument.Save("..\\..\\..\\TorrentRSS.xml");
        }

        private static void AddXML(string s, string m, string u)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("..\\..\\..\\TorrentRSS.xml");
            XmlNode channel = xmlDocument.SelectSingleNode("rss/channel");
            XmlNode item = xmlDocument.CreateElement("item");
            channel.AppendChild(item);
            XmlNode title = xmlDocument.CreateElement("title");
            item.AppendChild(title);
            XmlNode link = xmlDocument.CreateElement("link");
            item.AppendChild(link);
            XmlNode enclosure = xmlDocument.CreateElement("enclosure");
            item.AppendChild(enclosure);
            XmlAttribute url = xmlDocument.CreateAttribute("url");
            enclosure.Attributes.Append(url);
            XmlAttribute type = xmlDocument.CreateAttribute("type");
            enclosure.Attributes.Append(type);
            title.InnerText = s;
            link.InnerText = u;
            url.Value = m;
            type.Value = "application/x-bittorrent";
            xmlDocument.Save("..\\..\\..\\TorrentRSS.xml");
        }

        string GetDomain(string domain, string tld, int count)
        {
            while (true)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://" + domain + count + "." + tld);
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return "http://www." + domain + count + "." + tld;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    continue;
                }
                finally
                {
                    count--;
                }
            }
        }

        string GetHtml(string url)
        {
            using var client = new WebClient();
            return client.DownloadString(url);
        }
    }
}