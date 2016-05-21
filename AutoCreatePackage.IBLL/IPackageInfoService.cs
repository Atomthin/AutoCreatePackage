using AutoCreatePackage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreatePackage.IBLL
{
    public interface IPackageInfoService : IBaseService<Package>
    {
        string DownloadPackage(string urlAddress, string savePath, string packageName);
    }
}
