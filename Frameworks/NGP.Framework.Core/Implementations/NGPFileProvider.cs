/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPFileProvider Description:
 * NGP文件提供者(IO功能使用磁盘上的文件系统)
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;

namespace NGP.Framework.Core
{
    /// <summary>
    /// NGP文件提供者(IO功能使用磁盘上的文件系统)
    /// </summary>
    public class NGPFileProvider : PhysicalFileProvider, INGPFileProvider
    {
        /// <summary>
        /// 初始化NGPFileProvider的新实例
        /// </summary>
        /// <param name="hostingEnvironment">托管环境</param>
        public NGPFileProvider(string path = null)
            : base(string.IsNullOrWhiteSpace(path) ? AppContext.BaseDirectory : path)
        {
            if (File.Exists(path))
                path = Path.GetDirectoryName(path);

            BaseDirectory = path;
        }

        #region Utilities

        private static void DeleteDirectoryRecursive(string path)
        {
            Directory.Delete(path, true);
            const int maxIterationToWait = 10;
            var curIteration = 0;

            //according to the documentation(https://msdn.microsoft.com/ru-ru/library/windows/desktop/aa365488.aspx) 
            //System.IO.Directory.Delete method ultimately (after removing the files) calls native 
            //RemoveDirectory function which marks the directory as "deleted". That's why we wait until 
            //the directory is actually deleted. For more details see https://stackoverflow.com/a/4245121
            while (Directory.Exists(path))
            {
                curIteration += 1;
                if (curIteration > maxIterationToWait)
                    return;
                Thread.Sleep(100);
            }
        }

        #endregion

        /// <summary>
        /// 将字符串数组组合到路径中
        /// </summary>
        /// <param name="paths">路径部分的数组</param>
        /// <returns>组合路径</returns>
        public virtual string Combine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <summary>
        /// 在指定路径中创建所有目录和子目录，除非它们已经存在。
        /// </summary>
        /// <param name="path">要创建的目录</param>
        public virtual void CreateDirectory(string path)
        {
            if (!DirectoryExists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 在指定路径中创建或覆盖文件
        /// </summary>
        /// <param name="path">要创建的文件的路径和名称</param>
        public virtual void CreateFile(string path)
        {
            if (FileExists(path))
                return;

            //we use 'using' to close the file after it's created
            using (File.Create(path))
            {
            }
        }

        /// <summary>
        /// 深度优先递归删除，处理在Windows资源管理器中打开的子目录。
        /// </summary>
        /// <param name="path">目录路径</param>
        public void DeleteDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(path);

            //find more info about directory deletion
            //and why we use this approach at https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true

            foreach (var directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                DeleteDirectoryRecursive(path);
            }
            catch (IOException)
            {
                DeleteDirectoryRecursive(path);
            }
            catch (UnauthorizedAccessException)
            {
                DeleteDirectoryRecursive(path);
            }
        }

        /// <summary>
        /// 删除指定的文件
        /// </summary>
        /// <param name="filePath">要删除的文件的名称。不支持通配符</param>
        public virtual void DeleteFile(string filePath)
        {
            if (!FileExists(filePath))
                return;

            File.Delete(filePath);
        }

        /// <summary>
        /// 确定给定路径是否引用磁盘上的现有目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// 如果路径引用现有目录，则为true；如果目录不存在或在尝试确定指定文件是否存在时发生错误，则为false
        /// </returns>
        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 将文件或目录及其内容移动到新位置
        /// </summary>
        /// <param name="sourceDirName">要移动的文件或目录的路径</param>
        /// <param name="destDirName">
        /// sourcedirname的新位置的路径。如果sourcedirname是一个文件，那么destdirname也必须是一个文件名
        /// </param>
        public virtual void DirectoryMove(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }

        /// <summary>
        /// 返回与指定路径中的搜索模式匹配的文件名的可枚举集合，并可以选择搜索子目录。
        /// </summary>
        /// <param name="directoryPath">要搜索的目录的路径</param>
        /// <param name="searchPattern">
        /// 与路径中文件名匹配的搜索字符串。这个参数可以包含有效的文本路径和通配符（*and？）的组合。文字，但不支持正则表达式。
        /// </param>
        /// <param name="topDirectoryOnly">
        /// 指定是搜索当前目录，还是搜索当前目录和所有子目录
        /// </param>
        /// <returns>
        /// 由path指定的目录中的文件的全名（包括路径）的可枚举集合，这些文件与指定的搜索模式匹配。
        /// </returns>
        public virtual IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern,
            bool topDirectoryOnly = true)
        {
            return Directory.EnumerateFiles(directoryPath, searchPattern,
                topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /// <summary>
        /// 将现有文件复制到新文件。允许覆盖同名文件
        /// </summary>
        /// <param name="sourceFileName">要复制的文件</param>
        /// <param name="destFileName">目标文件的名称。这不能是目录</param>
        /// <param name="overwrite">如果可以覆盖目标文件，则为true；否则为false</param>
        public virtual void FileCopy(string sourceFileName, string destFileName, bool overwrite = false)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        /// <summary>
        /// 确定指定的文件是否存在
        /// </summary>
        /// <param name="filePath">要检查的文件</param>
        /// <returns>
        /// 如果调用方具有所需的权限，并且路径包含现有文件的名称，则为true；否则为false。
        /// </returns>
        public virtual bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 获取文件的长度（以字节为单位），对于目录或不存在的文件，则为-1
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件的长度</returns>
        public virtual long FileLength(string path)
        {
            if (!FileExists(path))
                return -1;

            return new FileInfo(path).Length;
        }

        /// <summary>
        /// 将指定的文件移动到新位置，提供指定新文件名的选项
        /// </summary>
        /// <param name="sourceFileName">要移动的文件的名称。可以包括相对路径或绝对路径</param>
        /// <param name="destFileName">文件的新路径和名称</param>
        public virtual void FileMove(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        /// <summary>
        /// 返回目录的绝对路径
        /// </summary>
        /// <param name="paths">路径部分的数组</param>
        /// <returns>目录的绝对路径</returns>
        public virtual string GetAbsolutePath(params string[] paths)
        {
            var allPaths = paths.ToList();
            allPaths.Insert(0, Root);

            return Path.Combine(allPaths.ToArray());
        }

        /// <summary>
        /// 获取用于封装指定目录的访问控制列表（ACL）项的System.Security.AccessControl.DirectorySecurity对象
        /// </summary>
        /// <param name="path">包含描述文件访问控制列表（ACL）信息的System.Security.AccessControl.DirectorySecurity对象的目录路径</param>
        /// <returns>封装由path参数描述的文件的访问控制规则的对象。</returns>
        public virtual DirectorySecurity GetAccessControl(string path)
        {
            return new DirectoryInfo(path).GetAccessControl();
        }

        /// <summary>
        /// 返回指定文件或目录的创建日期和时间
        /// </summary>
        /// <param name="path">获取创建日期和时间信息的文件或目录。</param>
        /// <returns>
        /// 设置为指定文件或目录的创建日期和时间的System.DateTime结构。该值以当地时间表示
        /// </returns>
        public virtual DateTime GetCreationTime(string path)
        {
            return File.GetCreationTime(path);
        }

        /// <summary>
        /// 返回与指定目录中指定搜索模式匹配的子目录（包括其路径）的名称
        /// </summary>
        /// <param name="path">要搜索的目录的路径</param>
        /// <param name="searchPattern">
        /// 与路径中子目录的名称匹配的搜索字符串。此参数可以包含有效的文字和通配符的组合，但不支持正则表达式。
        /// </param>
        /// <param name="topDirectoryOnly">
        /// 指定是搜索当前目录，还是搜索当前目录和所有子目录
        /// </param>
        /// <returns>
        /// 与指定条件匹配的子目录的全名（包括路径）数组；如果找不到目录，则为空数组。
        /// </returns>
        public virtual string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true)
        {
            if (string.IsNullOrEmpty(searchPattern))
                searchPattern = "*";

            return Directory.GetDirectories(path, searchPattern,
                topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /// <summary>
        /// 返回指定路径字符串的目录信息
        /// </summary>
        /// <param name="path">文件或目录的路径</param>
        /// <returns>
        /// 路径的目录信息，如果路径表示根目录或为空，则为空。如果路径不包含目录信息，则返回System.String.Empty
        /// </returns>
        public virtual string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// 仅返回指定路径字符串的目录名
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>目录名</returns>
        public virtual string GetDirectoryNameOnly(string path)
        {
            return new DirectoryInfo(path).Name;
        }

        /// <summary>
        /// 返回指定路径字符串的扩展名
        /// </summary>
        /// <param name="filePath">从中获取扩展名的路径字符串</param>
        /// <returns>指定路径的扩展名（包括句点“.”）。</returns>
        public virtual string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// 返回指定路径字符串的文件名和扩展名
        /// </summary>
        /// <param name="path">从中获取文件名和扩展名的路径字符串</param>
        /// <returns>路径中最后一个目录字符后的字符</returns>
        public virtual string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// 返回不带扩展名的指定路径字符串的文件名
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <returns>文件名，减去最后一个句点（.）及其后面的所有字符</returns>
        public virtual string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// 返回与指定目录中指定搜索模式匹配的文件名（包括其路径），使用值确定是否搜索子目录。
        /// </summary>
        /// <param name="directoryPath">要搜索的目录的路径</param>
        /// <param name="searchPattern">
        /// 与路径中文件名匹配的搜索字符串。此参数可以包含有效的文本路径和通配符（*and？）的组合。字符，但不支持正则表达式。
        /// </param>
        /// <param name="topDirectoryOnly">
        /// 指定是搜索当前目录，还是搜索当前目录和所有子目录
        /// </param>
        /// <returns>
        /// 指定目录中与指定搜索模式匹配的文件的全名（包括路径）数组；如果找不到文件，则为空数组。
        /// </returns>
        public virtual string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true)
        {
            if (string.IsNullOrEmpty(searchPattern))
                searchPattern = "*.*";

            return Directory.GetFiles(directoryPath, searchPattern,
                topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /// <summary>
        /// 返回上次访问指定文件或目录的日期和时间
        /// </summary>
        /// <param name="path">要获取访问日期和时间信息的文件或目录</param>
        /// <returns>System.DateTime结构设置为指定文件的日期和时间</returns>
        public virtual DateTime GetLastAccessTime(string path)
        {
            return File.GetLastAccessTime(path);
        }

        /// <summary>
        /// 返回上次写入指定文件或目录的日期和时间
        /// </summary>
        /// <param name="path">获取写入日期和时间信息的文件或目录。</param>
        /// <returns>
        /// 设置为指定文件或目录上次写入的日期和时间的System.DateTime结构。此值以本地时间表示
        /// </returns>
        public virtual DateTime GetLastWriteTime(string path)
        {
            return File.GetLastWriteTime(path);
        }

        /// <summary>
        /// 以协调世界时（UTC）返回指定文件或目录上次写入的日期和时间。
        /// </summary>
        /// <param name="path">获取写入日期和时间信息的文件或目录。</param>
        /// <returns>
        /// 设置为指定文件或目录上次写入的日期和时间的System.DateTime结构。此值以UTC时间表示。
        /// </returns>
        public virtual DateTime GetLastWriteTimeUtc(string path)
        {
            return File.GetLastWriteTimeUtc(path);
        }

        /// <summary>
        /// 检索指定路径的父目录
        /// </summary>
        /// <param name="directoryPath">检索父目录的路径</param>
        /// <returns>父目录，如果path是根目录，则为空，包括UNC服务器的根目录或共享名。</returns>
        public virtual string GetParentDirectory(string directoryPath)
        {
            return Directory.GetParent(directoryPath).FullName;
        }

        /// <summary>
        /// 检查路径是否为目录
        /// </summary>
        /// <param name="path">检查路径</param>
        /// <returns>如果路径是目录，则为true，否则为false</returns>
        public virtual bool IsDirectory(string path)
        {
            return DirectoryExists(path);
        }

        /// <summary>
        /// 将虚拟路径映射到物理磁盘路径
        /// </summary>
        /// <param name="path">“~/bin”</param>
        /// <returns>物理路径 path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string MapPath(string path)
        {
            path = path.Replace("~/", string.Empty).TrimStart('/').Replace('/', '\\');
            return Path.Combine(BaseDirectory ?? string.Empty, path);
        }

        /// <summary>
        /// 将文件内容读取到字节数组中
        /// </summary>
        /// <param name="filePath">用于读取的文件</param>
        /// <returns>包含文件内容的字节数组</returns>
        public virtual byte[] ReadAllBytes(string filePath)
        {
            return File.Exists(filePath) ? File.ReadAllBytes(filePath) : new byte[0];
        }

        /// <summary>
        /// 打开文件，用指定的编码读取文件的所有行，然后关闭文件。
        /// </summary>
        /// <param name="path">要打开以供读取的文件</param>
        /// <param name="encoding">应用于文件内容的编码</param>
        /// <returns>包含文件所有行的字符串</returns>
        public virtual string ReadAllText(string path, Encoding encoding)
        {
            return File.ReadAllText(path, encoding);
        }

        /// <summary>
        /// 以协调世界时（UTC）为单位设置上次写入指定文件的日期和时间。
        /// </summary>
        /// <param name="path">设置日期和时间信息的文件</param>
        /// <param name="lastWriteTimeUtc">
        /// 包含要为路径的最后一个写入日期和时间设置的值的System.DateTime。此值以UTC时间表示
        /// </param>
        public virtual void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
        }

        /// <summary>
        /// 将指定的字节数组写入文件
        /// </summary>
        /// <param name="filePath">写入的文件</param>
        /// <param name="bytes">文件字节数组</param>
        public virtual void WriteAllBytes(string filePath, byte[] bytes)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// 创建新文件，使用指定的编码将指定的字符串写入文件，然后关闭该文件。如果目标文件已经存在，它将被覆盖。
        /// </summary>
        /// <param name="path">要写入的文件</param>
        /// <param name="contents">要写入文件的字符串</param>
        /// <param name="encoding">要应用于字符串的编码</param>
        public virtual void WriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents, encoding);
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <typeparam name="T">反序列化对象</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>反序列化对象</returns>
        public T GetFileContent<T>(string filePath) where T : new()
        {
            var text = ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
                return new T();

            // 从JSON文件中获取插件系统名称
            return JsonConvert.DeserializeObject<T>(text);
        }

        /// <summary>
        /// 根路径
        /// </summary>
        protected string BaseDirectory { get; }
    }
}