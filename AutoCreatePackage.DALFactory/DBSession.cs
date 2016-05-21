using AutoCreatePackage.DAL;
using AutoCreatePackage.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreatePackage.DALFactory
{
    /// <summary>
    /// 数据会话层，实际上是一个工厂类，负责完成所有数据操作类实例的创建，然后业务层通过数据会话层来回去需要操作数据类的实例。所以数据会话层将业务层与数据层解耦。
    /// 在数据会话层中提供一个方法，完成所有数据的保存。
    /// </summary>
    public class DBSession : IDBSession
    {

        public DbContext Db
        {
            get { return DBContextFactory.CreateDbContext(); }
        }

        private IPackageInfoDal _packageInfoDal;

        public IPackageInfoDal PackageInfoDal
        {
            get
            {
                if (_packageInfoDal == null)
                {
                    _packageInfoDal = AbstractFactory.CreatePackageInfoDal();//通过抽象工厂封装了类的实例的创建
                }
                return _packageInfoDal;
            }
            set
            {
                _packageInfoDal = value;
            }
        }

        /// <summary>
        /// 一个业务中经常涉及到对多张表的操作，我们希望的是连接一次数据库，完成对多张表的操作。
        /// </summary>
        /// 工作单元模式
        /// <returns></returns>
        public bool SaveChanges()
        {
            return Db.SaveChanges() > 0;
        }
    }
}
