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
    }
}
