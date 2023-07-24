using System.Collections.ObjectModel;
using System.Linq;
using HtmlAgilityPack;

namespace OptifineDownloader.Utils.Parser
{
    abstract class Parser
    {
        public static string[] GetDownloadLinks(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//td[contains(@class, 'colDownload')]"); //Get nodes with download link
            Collection<string> versions = new Collection<string>();
            foreach(var node in nodes)
            {
                versions.Add(node.FirstChild.GetAttributeValue("href", null)); //get links from nodes
            }
            return versions.ToArray(); 
        }

        public static string GetFinalDownloadLink(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode element = doc.DocumentNode.SelectSingleNode("//span[contains(@id, 'Download')]/a"); //Get nodes with download link
            string link = "https://optifine.net/" + element.GetAttributeValue("href", null);
            return link;
        }
    }
}
