using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreatePackage.Tool
{
    public class GetHtmlNode
    {
        HtmlWeb web = new HtmlWeb();
        public HtmlNode GetHtmlNodes(string packageDownloadPageUrl, string htmlElementId, string packageXPath)
        {
            HtmlDocument doc = web.Load(packageDownloadPageUrl);
            HtmlNode node = null;
            node = doc.GetElementbyId(htmlElementId);
            return node;
        }
    }
}
