namespace AutoCreatePackage.Tool
{
    public interface IPackAndUnpack
    {
        string Zip(string filePath,string packageLatestVersion);
        string UnZip(string zipPath);
        string UnGZ(string gzPath);
        string UnTar(string tarPath);
    }
}
