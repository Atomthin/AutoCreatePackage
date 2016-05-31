using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoCreatePackage.WebApp.Models
{
    public class ACPExceptionAttribute : HandleErrorAttribute
    {
        public static Queue<Exception> ExceptionQueue = new Queue<Exception>();
        /// <summary>
        /// Catch the exception
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Exception ex = filterContext.Exception;
            ExceptionQueue.Enqueue(ex);
            filterContext.HttpContext.Response.Redirect("/Error.html");
        }
    }
}