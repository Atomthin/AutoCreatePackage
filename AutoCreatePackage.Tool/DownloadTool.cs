using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoCreatePackage.Tool
{

    public class DownloadTool
    {
        bool downLowdStatus = false;
        public string DownloadFile(string urlAddress, string savePath, string packageName)
        {
            string fileSavePath = null;
            string fileSaveFolderPath = null;
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);
                fileSaveFolderPath = string.Format(@"{0}\{1}", savePath, Regex.Replace(DateTime.Now.ToShortDateString(), @"\D+", ""));
                if (!Directory.Exists(fileSaveFolderPath))
                {
                    Directory.CreateDirectory(fileSaveFolderPath);
                }
                fileSavePath = fileSaveFolderPath + string.Format(@"\{0}.zip", packageName);
                try
                {
                    webClient.DownloadFile(URL, fileSavePath);
                }
                catch (Exception ex)
                {
                }

            }
            if (downLowdStatus)
            {
                return fileSavePath;
            }
            else
            {
                return null;
            }
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            downLowdStatus = true;
        }
    }

}
