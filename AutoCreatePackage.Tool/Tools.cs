using HtmlAgilityPack;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace AutoCreatePackage.Tool
{

    public class Tools : GetHtmlNode
    {
        IGetPackageDownloadUrl getPackageDownloadUrl;

        public void CheckVersionAndDownload()
        {

        }

        /// <summary>
        /// 判断软件包官网版本和本地版本
        /// </summary>
        /// <param name="packageDownloadPageUrl"></param>
        /// <param name="htmlElementId"></param>
        /// <param name="packageXPath"></param>
        /// <param name="currentVersion"></param>
        /// <returns></returns>
        private string CheckPackageVersion(string packageDownloadPageUrl, string htmlElementId, string packageXPath, string currentVersion)
        {
            HtmlNode node = this.GetHtmlNodes(packageDownloadPageUrl, htmlElementId, packageXPath);
            string temp = node.SelectSingleNode(packageXPath).InnerHtml;
            string pattern = @"(\d+(\.\d+)+)";
            Match m = Regex.Match(temp, pattern);
            if (!m.Success)
            {
                return null;
            }
            if (!string.Equals(m.Value, currentVersion, StringComparison.InvariantCultureIgnoreCase))
            {
                return m.Value;
            }
            return null;
        }

        /// <summary>
        /// 下载软件包方法
        /// </summary>
        /// <param name="urlAddress"></param>
        /// <param name="savePath"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public string DownloadFile(string urlAddress, string savePath, string packageName,string latestVersion)
        {
            string fileSavePath = null;
            string fileSaveFolderPath = null;
            using (WebClient webClient = new WebClient())
            {
                Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);
                fileSaveFolderPath = string.Format(@"{0}\{1}\{2}", savePath, packageName, latestVersion);
                if (!Directory.Exists(fileSaveFolderPath))
                {
                    Directory.CreateDirectory(fileSaveFolderPath);
                }
                fileSavePath = string.Format(@"{0}\{1}", fileSaveFolderPath, urlAddress.Split('/').Last());
                try
                {
                    webClient.DownloadFile(URL, fileSavePath);
                    return fileSavePath;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// 创建最新的软件包
        /// </summary>
        /// <param name="latestPackagePath"></param>
        /// <param name="configJson"></param>
        /// <returns></returns>
        private bool CreateLatestVersionPackage(string latestPackagePath,string currentPackagePath, string configJson)
        {
            
            return false;


        }
    }

}
