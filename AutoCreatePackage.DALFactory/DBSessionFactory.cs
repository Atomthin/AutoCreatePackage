using AutoCreatePackage.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreatePackage.DALFactory
{
    public class DBSessionFactory
    {
        public static IDBSession CreateDBSession()
        {
            IDBSession DbSession = (IDBSession)CallContext.GetData("dbSession");
            if (DbSession == null)
            {
                DbSession = new DBSession();
                CallContext.SetData("dbSession", DbSession);
            }
            return DbSession;

        }
    }
}
