using AutoCreatePackage.IBLL;
using AutoCreatePackage.Model;
using AutoCreatePackage.WebApp.Models;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web.Hosting;
using System.Web.Mvc;

namespace AutoCreatePackage.WebApp.Controllers
{
    public class PackageInfoController : Controller
    {
        [Dependency]
        public IPackageInfoService packageInfoService { get; set; }
        // GET: /PackageInfo/
        public ActionResult Index()
        {
            return View();
        }

        #region 获取软件包信息列表
        public ActionResult GetPackageInfoList()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["size"] != null ? int.Parse(Request["size"]) : 5;
            bool sortFlag = Convert.ToBoolean(Request["sort"] != null ? int.Parse(Request["sort"]) : 0);
            string searchStr = Request["search"];
            //Thread.Sleep(3000);
            Expression<Func<Package, bool>> searchLambda = p => p.Id > 0;
            if (searchStr != "null")
            {
                searchLambda = p => p.PackageName == searchStr;
            }
            int totalCount;
            var packageInfoList = packageInfoService.LoadPageEntities<DateTime>(pageIndex, pageSize, out totalCount, searchLambda, p => p.PackageUpdateTime, sortFlag);
            var temp = from p in packageInfoList
                       select new
                       {
                           Id = p.Id,
                           PId = p.PackageId,
                           PName = p.PackageName,
                           PDownLoadUrl = p.PackageDownloadPageUrl,
                           PStatus = p.PackageStatus,
                           PXPath = p.PackageXPath,
                           PHtmlElementId = p.HtmlElementId,
                           PHtmlElementAttr = p.HtmlElementAttr,
                           PDownloadUrlType = p.PackageDownloadUrlType,
                           PCurrentVersion = p.PackageCurrentVersion,
                           PSHA1Code = p.PackageSHA1Code,
                           PLastCheckDate = p.LastCheckDate,
                           PUpdateTime = p.PackageUpdateTime
                       };

            return Json(new { status = "ok", rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 添加软件包数据
        public ActionResult AddPackageInfo()
        {
            return PartialView("_AddPackage");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPackageInfo(PackageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Package package = new Package()
                {
                    PackageId = model.PackageId,
                    PackageName = model.PackageName,
                    PackageStatus = (int)StatusType.Normarl,
                    PackageDownloadPageUrl = model.PackageDownloadPageUrl,
                    PackageXPath = model.PackageXPath,
                    HtmlElementId = model.HtmlElementId,
                    HtmlElementAttr = model.HtmlElementAttr,
                    PackageDownloadUrlType = model.PackageDownloadUrlType,
                    PackageCurrentVersion = model.PackageCurrentVersion,
                    PackageUpdateTime = DateTime.Now,
                    LastCheckDate = DateTime.Now
                };
                package = packageInfoService.AddEntity(package);
                if (package.Id > 0)
                {
                    var temp = new
                    {
                        Id = package.Id,
                        PId = package.PackageId,
                        PName = package.PackageName,
                        PDownLoadUrl = package.PackageDownloadPageUrl,
                        PStatus = package.PackageStatus,
                        PXPath = package.PackageXPath,
                        PCurrentVersion = package.PackageCurrentVersion,
                        PLastCheckDate = package.LastCheckDate,
                        PUpdateTime = package.PackageUpdateTime
                    };
                    return Json(new { status = "ok", rows = temp });
                }
                else
                {
                    ModelState.AddModelError("", "添加失败！");
                }
            }
            return PartialView("_AddPackage", model);

        }
        #endregion

        #region 删除软件包
        public ActionResult DeletePackage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return PartialView("_DeletePackage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePackage(int id)
        {
            Package package = packageInfoService.LoadEntities(p => p.Id == id).FirstOrDefault();
            packageInfoService.DeleteEntity(package);
            return Json(new { status = "ok" });

        }
        #endregion

        #region 修改软件包信息
        public ActionResult EditPackage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = packageInfoService.LoadEntities(p => p.Id == id).FirstOrDefault();
            if (package == null)
            {
                return HttpNotFound();
            }
            return PartialView("_EditPackage", package);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPackage(Package model)
        {
            if (ModelState.IsValid)
            {
                if (packageInfoService.EditEntity(model))
                {
                    return Json(new { status = "ok" });
                }
                else
                {
                    ModelState.AddModelError("", "修改失败！");
                }

            }
            return PartialView("_EditPackage", model);
        }
        #endregion

    }
}