using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoCreatePackage.WebApp.Models
{
    public class PackageViewModel
    {
        [Required(ErrorMessage = "请输入软件包编号")]
        [Display(Name = "软件包编号")]
        public int PackageId { get; set; }

        [Required(ErrorMessage = "请输入软件包名字")]
        [Display(Name = "软件包名字")]
        public string PackageName { get; set; }

        [Required(ErrorMessage = "请输入软件包下载路径")]
        [Display(Name = "软件包下载路径")]
        public string PackageDownLoadUrl { get; set; }

        [Required(ErrorMessage = "请输入软件包的搜索XPath")]
        [Display(Name = "软件包搜索XPath")]
        public string PackageXPath { get; set; }

        [Required(ErrorMessage = "请输入软件包当前版本")]
        [Display(Name = "软件包当前版本")]
        public string PackageCurrentVersion { get; set; }
    }
}