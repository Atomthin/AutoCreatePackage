using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreatePackage.Tool
{
    public interface IPackAndUnPack
    {
        string Zip(string filePath);
        string UnZip(string zipPath);
        string UnGZ(string gzPath);
        string UnTar(string tarPath);
    }
}
