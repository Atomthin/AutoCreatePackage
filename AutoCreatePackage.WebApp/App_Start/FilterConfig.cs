using AutoCreatePackage.WebApp.Models;
using System.Web;
using System.Web.Mvc;

namespace AutoCreatePackage.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //uses log4net record log
            filters.Add(new ACPExceptionAttribute());
        }
    }
}
