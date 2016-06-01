using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "请输入软件包下载页面路径")]
        [Display(Name = "软件包下载页面路径")]
        public string PackageDownloadPageUrl { get; set; }

        [Required(ErrorMessage = "请输入软件包的搜索XPath")]
        [Display(Name = "软件包搜索XPath")]
        public string PackageXPath { get; set; }

        [Required(ErrorMessage = "请输入软件包HtmlElementId")]
        [Display(Name = "获取网页下载路径的Html Element Id")]
        public string HtmlElementId { get; set; }

        [Required(ErrorMessage = "请输入软件包HtmlElementAttr")]
        [Display(Name = "获取网页下载路径的Html Element Attribute")]
        public string HtmlElementAttr { get; set; }

        [Required(ErrorMessage = "请输入软件包下载路径的获取方式")]
        [Display(Name = "网页获请输入软件包下载路径的获取方式, 0:直接从属性获取, 1:从属性获取再拼接")]
        public int PackageDownloadUrlType { get; set; }

        [Required(ErrorMessage = "请输入软件包当前版本")]
        [Display(Name = "软件包当前版本")]
        public string PackageCurrentVersion { get; set; }
    }
}