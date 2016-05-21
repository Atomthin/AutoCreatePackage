using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreatePackage.IDAL
{
    /// <summary>
    /// 业务层调用数据会话层接口
    /// </summary>
    public interface IDBSession
    {
        DbContext Db { get; }
        IPackageInfoDal PackageInfoDal { get; set; }
        bool SaveChanges();
    }
}
