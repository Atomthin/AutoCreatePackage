using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Xml;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Configuration;

namespace AutoCreatePackage.Tool
{

    public class PackagesTools : GetHtmlNode
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
        public string CheckPackageVersion(string packageDownloadPageUrl, string htmlElementId, string packageXPath, string currentVersion, string htmlElementAttr, string packageName, int urlType)
        {
            string downloadUrl = string.Empty;
            string savePath = string.Empty;
            string latestPackagePath = string.Empty;
            string newZipPath = string.Empty;
            string newZipSHA1 = string.Empty;
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
                switch (urlType)
                {
                    case 0:
                        getPackageDownloadUrl = new GetDownloadUrlFromAttr();
                        break;
                    case 1:
                        getPackageDownloadUrl = new SplicePackageDownloadUrlFromAttr();
                        break;
                }
                downloadUrl = getPackageDownloadUrl.GetPackageDownloadUrl(packageDownloadPageUrl, htmlElementId, packageXPath, htmlElementAttr);
                savePath = ConfigurationManager.AppSettings["packageSavePath"].ToString();
                latestPackagePath = DownloadFile(downloadUrl, savePath, packageName, m.Value);
                newZipPath = CreateLatestVersionPackage(latestPackagePath, packageName, m.Value);
                newZipSHA1 = GenerateSHA1Code(newZipPath);
                //To do. Save record to DB.

                return JsonConvert.SerializeObject(new { ZipPath = newZipPath, SHA1Code = newZipSHA1 });
            }
            return null;
        }
        #endregion

        #region Download latest package
        /// <summary>
        /// DownloadFile
        /// </summary>
        /// <param name="downloaduUrl"></param>
        /// <param name="savePath"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        private string DownloadFile(string downloaduUrl, string savePath, string packageName, string latestVersion)
        {
            string fileSavePath = null;
            string fileSaveFolderPath = null;
            using (WebClient webClient = new WebClient())
            {
                Uri URL = downloaduUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(downloaduUrl) : new Uri("http://" + downloaduUrl);
                fileSaveFolderPath = HostingEnvironment.MapPath(string.Format("~/{0}/{1}/{2}", savePath, packageName, latestVersion));
                if (!Directory.Exists(fileSaveFolderPath))
                {
                    Directory.CreateDirectory(fileSaveFolderPath);
                }
                fileSavePath = HostingEnvironment.MapPath(string.Format("{0}/{1}", fileSaveFolderPath, downloaduUrl.Split('/').Last()));
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
            if (!File.Exists(configJsonPath))
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

        #region Generate SHA1 Code
        /// <summary>
        /// GenerateSHA1Code
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GenerateSHA1Code(string filePath)
        {
            string hashText = "";
            string hexValue = "";
            byte[] fileData = File.ReadAllBytes(filePath);
            byte[] hashData = SHA1.Create().ComputeHash(fileData); // SHA1 or MD5
            foreach (byte b in hashData)
            {
                hexValue = b.ToString("X").ToLower(); // Lowercase for compatibility on case-sensitive systems
                hashText += (hexValue.Length == 1 ? "0" : "") + hexValue;
            }
            return hashText;
        }
        #endregion
    }

}
