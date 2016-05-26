using HtmlAgilityPack;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace AutoCreatePackage.Tool
{

    public class Tools : GetHtmlNode
    {
        IGetPackageDownloadUrl getPackageDownloadUrl;
        IPackAndUnpack packAndUnPack;

        public void CheckVersionAndDownload()
        {

        }

        #region Check Package Version
        /// <summary>
        /// CheckPackageVersion
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
        #endregion

        #region Download latest package
        /// <summary>
        /// DownloadFile
        /// </summary>
        /// <param name="urlAddress"></param>
        /// <param name="savePath"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        private string DownloadFile(string urlAddress, string savePath, string packageName, string latestVersion)
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
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
        }
        #endregion

        #region Create latest version package
        /// <summary>
        /// Create latest version package
        /// </summary>
        /// <param name="latestPackagePath"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        private bool CreateLatestVersionPackage(string latestPackagePath, string packageName)
        {
            packAndUnPack = new PackAndUnpack();
            string extensionName = Path.GetExtension(latestPackagePath);
            string unpackPath = null;
            switch (extensionName)
            {
                case ".zip":
                    unpackPath = packAndUnPack.UnZip(latestPackagePath);
                    break;
                case ".gz":
                    string tempPath = packAndUnPack.UnGZ(latestPackagePath);
                    unpackPath = packAndUnPack.UnTar(tempPath);
                    break;
            }
            string configJsonPath=HostingEnvironment.MapPath(string.Format("~/Requirement/{0}", packageName));
            if(File.Exists(configJsonPath))
            {
                return false;
            }
            string getConfigJson = File.ReadAllText(configJsonPath).Trim();


            



            return false;
        }
        #endregion
    }

}
