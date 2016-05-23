using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreatePackage.Tool
{
    public interface IGetPackageDownloadUrl
    {
        string GetPackageDownloadUrl(string packageDownloadPageUrl, string htmlElementId, string packageXPath, string htmlElementAttr);
    }
}
