using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreatePackage.Tool
{
    public class PackAndUnPack : IPackAndUnPack
    {
        public string Zip(string filePath, string zipFolderDir)
        {
            string packFilePath = null;
            try
            {
                if (!Directory.Exists(zipFolderDir))
                {
                    Directory.CreateDirectory(zipFolderDir);
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                FastZip fz = new FastZip();
                fz.CreateEmptyDirectories = true;
                fz.CreateZip(filePath, zipFolderDir, true, "");
                packFilePath = string.Format(@"{0}\{1}.zip", zipFolderDir, zipFolderDir.Substring(zipFolderDir.LastIndexOf(@"\") + 1, zipFolderDir.Length));
            }
            catch (Exception e)
            {
                packFilePath = null;
                throw new Exception(e.Message);
            }
            return packFilePath;

        }

        public string UnZip(string zipPath, string unZipDir)
        {
            string unZipFolderPath = null;
            try
            {
                if (!File.Exists(zipPath))
                {
                    return null;
                }
                if (!Directory.Exists(unZipDir))
                {
                    Directory.CreateDirectory(unZipDir);
                }
                FileStream fr = new FileStream(zipPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                ZipInputStream s = new ZipInputStream(fr);
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName != String.Empty)
                    {
                        Directory.CreateDirectory(unZipDir + directoryName);
                    }
                    if (fileName != String.Empty)
                    {
                        unZipFolderPath = unZipDir + theEntry.Name;
                        FileStream streamWriter = File.Create(unZipFolderPath);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
                s.Close();
                fr.Close();
            }
            catch (Exception e)
            {
                unZipFolderPath = null;
                throw new Exception(e.Message);
            }
            return unZipFolderPath;

        }

        public string UnGZ(string gzPath, string unPackDir)
        {
            string unGZFolderPath = null;
            try
            {
                FileInfo fileInfo = new FileInfo(gzPath);
                using (FileStream originalFileStream = fileInfo.OpenRead())
                {
                    string currentFileName = fileInfo.FullName;
                    string newFileName = currentFileName.Remove(currentFileName.Length - fileInfo.Extension.Length);

                    using (FileStream decompressedFileStream = File.Create(newFileName))
                    {
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(decompressedFileStream);
                            unGZFolderPath = gzPath.Substring(0, gzPath.LastIndexOf(@"\") + 1);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                unGZFolderPath = null;
                throw new Exception(e.Message);
            }
            return unGZFolderPath;
        }

        public string UnTar(string tarPath, string unPackDir)
        {
            string unTarFolderPath = null;
            try
            {
                if (!File.Exists(tarPath))
                {
                    return null;
                }
                if (!Directory.Exists(unPackDir))
                {
                    Directory.CreateDirectory(unPackDir);
                }
                FileStream fr = new FileStream(tarPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                TarInputStream s = new ICSharpCode.SharpZipLib.Tar.TarInputStream(fr);
                TarEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName != String.Empty)
                    {
                        Directory.CreateDirectory(unPackDir + directoryName);
                    }  
                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(unPackDir + theEntry.Name);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
                s.Close();
                fr.Close();
                unTarFolderPath=
            }
            catch (Exception e)
            {
                unTarFolderPath = null;
                throw new Exception(e.Message);
            }
            return unTarFolderPath;
        }
    }
}
