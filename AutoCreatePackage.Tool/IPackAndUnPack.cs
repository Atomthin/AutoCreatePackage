using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreatePackage.Tool
{
    public interface IPackAndUnPack
    {
        string Zip(string filePath, string zipFolderDir);
        string UnZip(string zipPath,string unZipDir);
        string UnGZ(string gzPath,string unGZDir);
        string UnTar(string tarPath, string unTarDir);
    }
}
