using ICSharpCode.SharpZipLib.Tar;
using System;
using System.IO;
using System.IO.Compression;

namespace AutoCreatePackage.Tool
{
    public class PackAndUnPack : IPackAndUnPack
    {
        #region Create zip
        /// <summary>
        /// Pack file to zip package.
        /// </summary>
        /// <param name="filePath">File path which need to pack.</param>
        /// <returns></returns>
        public string Zip(string filePath)
        {
            try
            {
                if (!Directory.Exists(filePath))
                {
                    return null;
                }
                string packFilePath = string.Format(@"{0}\{1}.zip", filePath.Substring(0, filePath.LastIndexOf(@"\")), Path.GetFileName(filePath));
                ZipFile.CreateFromDirectory(filePath, packFilePath);
                return packFilePath;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region Extract zip
        /// <summary>
        /// Extract zip package
        /// </summary>
        /// <param name="zipPath">The zip package file path.</param>
        /// <returns></returns>
        public string UnZip(string zipPath)
        {
            try
            {
                if (!File.Exists(zipPath))
                {
                    return null;
                }
                string extractPath = zipPath.Substring(0, zipPath.LastIndexOf(@"\"));
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                return extractPath;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region Extract gz
        /// <summary>
        /// Extract gz package
        /// </summary>
        /// <param name="gzPath">The gz package file path.</param>
        /// <returns></returns>
        public string UnGZ(string gzPath)
        {
            try
            {
                if (!File.Exists(gzPath))
                {
                    return null;
                }
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
                            return newFileName;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region Extract tar
        /// <summary>
        /// Extract tar package
        /// </summary>
        /// <param name="tarPath">The tar file path.</param>
        /// <returns></returns>
        public string UnTar(string tarPath)
        {
            try
            {
                string strUnpackPath = null;
                if (!File.Exists(tarPath))
                {
                    return null;
                }
                string strUnpackDir = tarPath.Substring(0, tarPath.LastIndexOf('.')).Replace("/", "\\");
                if (!strUnpackDir.EndsWith("\\"))
                {
                    strUnpackDir += "\\";
                }
                if (!Directory.Exists(strUnpackDir))
                {
                    Directory.CreateDirectory(strUnpackDir);
                }
                FileStream fr = new FileStream(tarPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                TarInputStream s = new TarInputStream(fr);
                TarEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName != String.Empty)
                    {
                        strUnpackPath = strUnpackDir + directoryName;
                        Directory.CreateDirectory(strUnpackPath);
                    }
                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(strUnpackDir + theEntry.Name);

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
                return strUnpackPath;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        } 
        #endregion
    }
}
