using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace UNote
{
    public class CommonHelper
    {
        /// <summary>
        /// 字节转换M（如：将481512B 转成 470K）
        /// </summary>
        /// <param name="byteLength"></param>
        /// <returns></returns>
        //public static  string ByteToM(int byteLength, string mstr = "M")
        //{
        //    if (byteLength > 0)
        //        return Math.Round(((byteLength / 1024f) / 1024f), 1) + mstr;
        //    else
        //        return "0" + mstr;
        //}

        public static String FormatFileSize(Int64 fileSize)
        {
            if (fileSize < 0)
            {
                throw new ArgumentOutOfRangeException("fileSize");
            }
            else if (fileSize >= 1024 * 1024 * 1024)
            {
                return string.Format("{0:########0.00} GB", ((Double)fileSize) / (1024 * 1024 * 1024));
            }
            else if (fileSize >= 1024 * 1024)
            {
                return string.Format("{0:####0.00} M", ((Double)fileSize) / (1024 * 1024));
            }
            else if (fileSize >= 1024)
            {
                return string.Format("{0:####0.00} KB", ((Double)fileSize) / 1024);
            }
            else
            {
                return string.Format("{0} b", fileSize);
            }
        }

        public static string FormatTimeNote(DateTime longtime, string defaultFormatTime)
        {
            TimeSpan ts = DateTime.Now - longtime;
            if (ts >= new TimeSpan(1, 0, 0, 0) && ts.TotalDays < 6)
            {
                return String.Format("{0:N0}天前", ts.TotalDays);
            }
            else if (ts >= new TimeSpan(1, 0, 0, 0) && ts.TotalDays >= 6)
            {
                return defaultFormatTime;
            }
            else if (ts >= new TimeSpan(1, 0, 0))
            {
                return String.Format("{0:N0}小时前", ts.TotalHours);
            }
            else if (ts >= new TimeSpan(0, 1, 0))
            {
                return String.Format("{0:N0}分钟前", ts.TotalMinutes);
            }
            else
            {
                return "刚刚";
            }
        }

        #region Zip
        /// <summary>  
        /// 压缩单个文件  
        /// </summary>  
        /// <param name="FileToZip">被压缩的文件名称(包含文件路径)</param>  
        /// <param name="ZipedFile">压缩后的文件名称(包含文件路径)</param>  
        /// <param name="CompressionLevel">压缩率0（无压缩）-9（压缩率最高）</param>  
        /// <param name="BlockSize">缓存大小</param>  
        public void ZipFile(string FileToZip, string ZipedFile, int CompressionLevel)
        {
            //如果文件没有找到，则报错   
            if (!System.IO.File.Exists(FileToZip))
            {
                throw new System.IO.FileNotFoundException("文件：" + FileToZip + "没有找到！");
            }

            if (ZipedFile == string.Empty)
            {
                ZipedFile = Path.GetFileNameWithoutExtension(FileToZip) + ".zip";
            }

            if (Path.GetExtension(ZipedFile) != ".zip")
            {
                ZipedFile = ZipedFile + ".zip";
            }

            ////如果指定位置目录不存在，创建该目录  
            //string zipedDir = ZipedFile.Substring(0,ZipedFile.LastIndexOf("/"));  
            //if (!Directory.Exists(zipedDir))  
            //    Directory.CreateDirectory(zipedDir);  

            //被压缩文件名称  
            string filename = FileToZip.Substring(FileToZip.LastIndexOf('/') + 1);

            System.IO.FileStream StreamToZip = new System.IO.FileStream(FileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.FileStream ZipFile = System.IO.File.Create(ZipedFile);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            ZipEntry ZipEntry = new ZipEntry(filename);
            ZipStream.PutNextEntry(ZipEntry);
            ZipStream.SetLevel(CompressionLevel);
            byte[] buffer = new byte[2048];
            System.Int32 size = StreamToZip.Read(buffer, 0, buffer.Length);
            ZipStream.Write(buffer, 0, size);
            try
            {
                while (size < StreamToZip.Length)
                {
                    int sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                    ZipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                ZipStream.Finish();
                ZipStream.Close();
                StreamToZip.Close();
            }
        }

        /// <summary>  
        /// 压缩文件夹的方法  
        /// </summary>  
        public void ZipDir(string DirToZip, string ZipedFile, int CompressionLevel)
        {
            //压缩文件为空时默认与压缩文件夹同一级目录  
            if (ZipedFile == string.Empty)
            {
                ZipedFile = DirToZip.Substring(DirToZip.LastIndexOf("/") + 1);
                ZipedFile = DirToZip.Substring(0, DirToZip.LastIndexOf("/")) + "//" + ZipedFile + ".zip";
            }

            if (Path.GetExtension(ZipedFile) != ".zip")
            {
                ZipedFile = ZipedFile + ".zip";
            }

            using (ZipOutputStream zipoutputstream = new ZipOutputStream(File.Create(ZipedFile)))
            {
                zipoutputstream.SetLevel(CompressionLevel);
                Crc32 crc = new Crc32();
                Hashtable fileList = getAllFies(DirToZip);
                foreach (DictionaryEntry item in fileList)
                {
                    FileStream fs = File.OpenRead(item.Key.ToString());
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ZipEntry entry = new ZipEntry(item.Key.ToString().Substring(DirToZip.Length + 1));
                    entry.DateTime = (DateTime)item.Value;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipoutputstream.PutNextEntry(entry);
                    zipoutputstream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>  
        /// 获取所有文件  
        /// </summary>  
        /// <returns></returns>  
        private Hashtable getAllFies(string dir)
        {
            Hashtable FilesList = new Hashtable();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new System.IO.FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }

            this.getAllDirFiles(fileDire, FilesList);
            this.getAllDirsFiles(fileDire.GetDirectories(), FilesList);
            return FilesList;
        }
        /// <summary>  
        /// 获取一个文件夹下的所有文件夹里的文件  
        /// </summary>  
        /// <param name="dirs"></param>  
        /// <param name="filesList"></param>  
        private void getAllDirsFiles(DirectoryInfo[] dirs, Hashtable filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                foreach (FileInfo file in dir.GetFiles("*.*"))
                {
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                this.getAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }
        /// <summary>  
        /// 获取一个文件夹下的文件  
        /// </summary>  
        /// <param name="strDirName">目录名称</param>  
        /// <param name="filesList">文件列表HastTable</param>  
        private void getAllDirFiles(DirectoryInfo dir, Hashtable filesList)
        {
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }

        /// <summary>  
        /// 功能：解压zip格式的文件。  
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径</param>  
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>  
        /// <param name="err">出错信息</param>  
        /// <returns>解压是否成功</returns>  
        public static void UnZip(string zipFilePath, string unZipDir)
        {
            if (zipFilePath == string.Empty)
            {
                throw new Exception("压缩文件不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new System.IO.FileNotFoundException("压缩文件不存在！");
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("/"))
                unZipDir += "/";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(unZipDir + directoryName);
                    }
                    if (!directoryName.EndsWith("/"))
                        directoryName += "/";
                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                        {

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
                        }
                    }
                }
            }
        }
        #endregion
    }
}
