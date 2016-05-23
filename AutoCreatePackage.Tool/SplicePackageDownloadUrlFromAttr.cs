using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreatePackage.Tool
{
    public class SplicePackageDownloadUrlFromAttr : GetHtmlNode, IGetPackageDownloadUrl
    {
        public string GetPackageDownloadUrl(string packageDownloadPageUrl, string htmlElementId, string packageXPath, string htmlElementAttr)
        {
            HtmlNode node = this.GetHtmlNodes(packageDownloadPageUrl, htmlElementId, packageXPath);
            string partialUrl = node.SelectSingleNode("//a[@class=\"button download-button button-large button-large\"]/strong").Attributes[htmlElementId].Value;
            return string.Format("{0}{1}", packageDownloadPageUrl.Split('/').First(), partialUrl);
        }
    }
}
