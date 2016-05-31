using HtmlAgilityPack;

namespace AutoCreatePackage.Tool
{
    public class GetHtmlNode
    {
        HtmlWeb web = new HtmlWeb();
        public HtmlNode GetHtmlNodes(string packageDownloadPageUrl, string htmlElementId)
        {
            HtmlDocument doc = web.Load(packageDownloadPageUrl);
            HtmlNode node = null;
            node = doc.GetElementbyId(htmlElementId);
            return node;
        }
    }
}
