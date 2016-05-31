using HtmlAgilityPack;
using System.Linq;

namespace AutoCreatePackage.Tool
{
    public class SplicePackageDownloadUrlFromAttr : GetHtmlNode, IGetPackageDownloadUrl
    {
        public string GetPackageDownloadUrl(string packageDownloadPageUrl, string htmlElementId, string packageXPath, string htmlElementAttr)
        {
            HtmlNode node = this.GetHtmlNodes(packageDownloadPageUrl, htmlElementId);
            string partialUrl = node.SelectSingleNode(packageXPath).Attributes[htmlElementAttr].Value;
            return string.Format("{0}{1}", packageDownloadPageUrl.Split('/').First(), partialUrl);
        }
    }
}
