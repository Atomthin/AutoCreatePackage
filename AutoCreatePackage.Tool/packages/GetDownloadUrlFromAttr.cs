using HtmlAgilityPack;

namespace AutoCreatePackage.Tool
{
    public class GetDownloadUrlFromAttr : GetHtmlNode, IGetPackageDownloadUrl
    {
        public string GetPackageDownloadUrl(string packageDownloadPageUrl, string htmlElementId, string packageXPath, string htmlElementAttr)
        {
            HtmlNode node = this.GetHtmlNodes(packageDownloadPageUrl, htmlElementId);
            return node.SelectSingleNode(packageXPath).Attributes[htmlElementAttr].Value;
        }
    }
}
