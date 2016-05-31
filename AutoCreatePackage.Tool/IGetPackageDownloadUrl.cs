namespace AutoCreatePackage.Tool
{
    public interface IGetPackageDownloadUrl
    {
        string GetPackageDownloadUrl(string packageDownloadPageUrl, string htmlElementId, string packageXPath, string htmlElementAttr);
    }
}
