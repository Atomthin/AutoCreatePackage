using AutoCreatePackage.IBLL;
using AutoCreatePackage.Model;
using AutoCreatePackage.Tool;


namespace AutoCreatePackage.BLL
{
    public class PackageInfoService : BaseService<Package>, IPackageInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.PackageInfoDal;
        }

        public string DownloadPackage(string urlAddress, string savePath, string packageName)
        {
            DownloadTool dlt = new DownloadTool();
            string packageSavePath = dlt.DownloadFile(urlAddress, savePath, packageName);
            return packageSavePath;
        }
    }
}
