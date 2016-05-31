using System.Collections.Generic;

namespace AutoCreatePackage.Tool
{
    public class PackageConfig
    {
        public string packageName { get; set; }
        public List<ReplaceFile> replaceFile { get; set; }
        public List<ModifyFile> modifyFile { get; set; }
    }

    public class ReplaceFile
    {
        public string oldFilePath { get; set; }
        public string newFilePath { get; set; }
    }

    public class ModifyFile
    {
        public string filePath { get; set; }
        public string xPath { get; set; }
        public string attName { get; set; }
        public string modifyContent { get; set; }
    }
}
