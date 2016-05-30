using HtmlAgilityPack;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Xml;
using Newtonsoft.Json;

namespace AutoCreatePackage.Tool
{

    public class Tools : GetHtmlNode
    {
        IGetPackageDownloadUrl getPackageDownloadUrl;
        IPackAndUnpack packAndUnPack;


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
            HtmlNode node = this.GetHtmlNodes(packageDownloadPageUrl, htmlElementId);
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
        /// <param name="packageLatestVersion"></param>
        /// <returns></returns>
        private string CreateLatestVersionPackage(string latestPackagePath, string packageName, string packageLatestVersion)
        {
            try
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
                string configJsonPath = HostingEnvironment.MapPath(string.Format("~/Requirement/{0}/config.json", packageName));
                if (File.Exists(configJsonPath))
                {
                    return null;
                }
                string getConfigJson = File.ReadAllText(configJsonPath).Trim();
                PackageConfig jObject = JsonConvert.DeserializeObject<PackageConfig>(getConfigJson);
                if (jObject.replaceFile.Count > 0)
                {
                    foreach (var item in jObject.replaceFile)
                    {
                        File.Copy(Path.Combine(unpackPath, item.oldFilePath), Path.Combine(unpackPath, item.newFilePath), true);
                    }
                }
                if (jObject.modifyFile.Count > 0)
                {
                    foreach (var item in jObject.modifyFile)
                    {
                        ModifyFile(Path.Combine(unpackPath, item.filePath), Path.Combine(unpackPath, item.xPath), item.attName, item.modifyContent);
                    }
                }
                return packAndUnPack.Zip(unpackPath, packageLatestVersion);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Modify File
        /// <summary>
        /// ModifyFile
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="xPath"></param>
        /// <param name="attName"></param>
        /// <param name="modifyContent"></param>
        private void ModifyFile(string filePath, string xPath, string attName, string modifyContent)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNode node = xmlDoc.SelectSingleNode(xPath);
            node.Attributes[attName].Value = modifyContent;
            xmlDoc.Save(filePath);
        } 
        #endregion
    }

}
