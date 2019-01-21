/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * PluginManager Description:
 * 插件管理
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Newtonsoft.Json;
using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

//Contributor: Umbraco (http://www.umbraco.com). Thanks a lot! 
//SEE THIS POST for full details of what this does - http://shazwazza.com/post/Developing-a-plugin-framework-in-ASPNET-with-medium-trust.aspx

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 插件管理
    /// </summary>
    public class PluginManager
    {
        #region Fields

        private static readonly INGPFileProvider _fileProvider;

        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        private static readonly List<string> _baseAppLibraries;
        private static string _shadowCopyFolder;
        private static string _reserveShadowCopyFolder;

        #endregion

        #region Ctor

        static PluginManager()
        {
            // 因为DI尚未初始化,所以使用默认文件提供程序
            _fileProvider = CommonHelper.DefaultFileProvider;

            // 获取所有dll
            _baseAppLibraries = _fileProvider.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Select(fi => _fileProvider.GetFileName(fi)).ToList();

            // 从基站目录中获取所有库
            if (!AppDomain.CurrentDomain.BaseDirectory.Equals(Environment.CurrentDirectory, StringComparison.InvariantCultureIgnoreCase))
                _baseAppLibraries.AddRange(_fileProvider.GetFiles(Environment.CurrentDirectory, "*.dll").Select(fi => _fileProvider.GetFileName(fi)));

            // 从refs目录中获取所有库
            var refsPathName = _fileProvider.Combine(Environment.CurrentDirectory, NGPPluginDefaults.RefsPathName);
            if (_fileProvider.DirectoryExists(refsPathName))
                _baseAppLibraries.AddRange(_fileProvider.GetFiles(refsPathName, "*.dll").Select(fi => _fileProvider.GetFileName(fi)));
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 获取描述文件
        /// </summary>
        /// <param name="pluginFolder">插件目录信息</param>
        /// <returns>原始和解析的描述文件</returns>
        private static IEnumerable<KeyValuePair<string, PluginDescriptor>> GetDescriptionFilesAndDescriptors(string pluginFolder)
        {
            if (pluginFolder == null)
                throw new ArgumentNullException(nameof(pluginFolder));

            var result = new List<KeyValuePair<string, PluginDescriptor>>();

            // 添加显示顺序和列表路径
            foreach (var descriptionFile in _fileProvider.GetFiles(pluginFolder, NGPPluginDefaults.DescriptionFileName, false))
            {
                if (!IsPackagePluginFolder(_fileProvider.GetDirectoryName(descriptionFile)))
                    continue;

                // 解析文件
                var pluginDescriptor = GetPluginDescriptorFromFile(descriptionFile);

                // 填充列表
                result.Add(new KeyValuePair<string, PluginDescriptor>(descriptionFile, pluginDescriptor));
            }

            // 按显示顺序排序列表。 注意：最低的DisplayOrder将是第一个，即0,1,1,1,5,10
            result.Sort((firstPair, nextPair) => firstPair.Value.DisplayOrder.CompareTo(nextPair.Value.DisplayOrder));
            return result;
        }

        /// <summary>
        /// 获取已安装插件的系统名称
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <returns>插件系统名称列表</returns>
        private static IList<string> GetInstalledPluginNames(string filePath)
        {
            var text = _fileProvider.ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
                return new List<string>();

            // 从JSON文件中获取插件系统名称
            return JsonConvert.DeserializeObject<IList<string>>(text);
        }

        /// <summary>
        /// 将已安装插件的系统名称保存到文件中
        /// </summary>
        /// <param name="pluginSystemNames">插件系统名称列表</param>
        /// <param name="filePath">文件的路径</param>
        private static void SaveInstalledPluginNames(IList<string> pluginSystemNames, string filePath)
        {
            // 保存文件
            var text = JsonConvert.SerializeObject(pluginSystemNames, Formatting.Indented);
            _fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// 指示是否已加载程序集文件
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>如果已加载文件，则为True; 否则是假的</returns>
        private static bool IsAlreadyLoaded(string filePath)
        {
            //在基本目录中搜索库文件名以忽略已存在（已加载）的库
            //（我们这样做是因为并非所有库都在应用程序启动后立即加载）
            if (_baseAppLibraries.Any(sli => sli.Equals(_fileProvider.GetFileName(filePath), StringComparison.InvariantCultureIgnoreCase)))
                return true;

            // 比较完整的装配名称
            //var fileAssemblyName = AssemblyName.GetAssemblyName(filePath);
            //foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    if (a.FullName.Equals(fileAssemblyName.FullName, StringComparison.InvariantCultureIgnoreCase))
            //        return true;
            //}
            //return false;

            // 不要比较完整的程序集名称，只是文件名
            try
            {
                var fileNameWithoutExt = _fileProvider.GetFileNameWithoutExtension(filePath);
                if (string.IsNullOrEmpty(fileNameWithoutExt))
                    throw new Exception($"Cannot get file extension for {_fileProvider.GetFileName(filePath)}");

                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var assemblyName = a.FullName.Split(',').FirstOrDefault();
                    if (fileNameWithoutExt.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Cannot validate whether an assembly is already loaded. " + exc);
            }

            return false;
        }

        /// <summary>
        /// 执行文件部署
        /// </summary>
        /// <param name="plug">插件文件信息</param>
        /// <param name="applicationPartManager"></param>
        /// <param name="config">Config</param>
        /// <param name="shadowCopyPath">Shadow copy path</param>
        /// <returns>Assembly</returns>
        private static Assembly PerformFileDeploy(string plug, ApplicationPartManager applicationPartManager, string shadowCopyPath = "")
        {
            var parent = string.IsNullOrEmpty(plug) ? string.Empty : _fileProvider.GetParentDirectory(plug);

            if (string.IsNullOrEmpty(parent))
                throw new InvalidOperationException($"The plugin directory for the {_fileProvider.GetFileName(plug)} file exists in a folder outside of the allowed nopCommerce folder hierarchy");

            //  为了避免可能的问题，我们仍然将库复制到 ~/Plugins/bin/ directory
            if (string.IsNullOrEmpty(shadowCopyPath))
                shadowCopyPath = _shadowCopyFolder;

            _fileProvider.CreateDirectory(shadowCopyPath);
            var shadowCopiedPlug = ShadowCopyFile(plug, shadowCopyPath);

            Assembly shadowCopiedAssembly = null;

            try
            {
                shadowCopiedAssembly = RegisterPluginDefinition(applicationPartManager, shadowCopiedPlug);
            }
            catch (FileLoadException)
            {
                if (!shadowCopyPath.Equals(_shadowCopyFolder))
                    throw;
            }

            return shadowCopiedAssembly ?? PerformFileDeploy(plug, applicationPartManager,_reserveShadowCopyFolder);
        }

        /// <summary>
        /// 注册插件定义
        /// </summary>
        /// <param name="config">Config</param>
        /// <param name="applicationPartManager">Application part manager</param>
        /// <param name="plug">Plugin file info</param>
        /// <returns>Assembly</returns>
        private static Assembly RegisterPluginDefinition(ApplicationPartManager applicationPartManager, string plug)
        {
            Assembly pluginAssembly;
            try
            {
                pluginAssembly = Assembly.LoadFrom(plug);
            }
            catch (FileLoadException)
            {
                throw;
            }

            applicationPartManager.ApplicationParts.Add(new AssemblyPart(pluginAssembly));

            return pluginAssembly;
        }

        /// <summary>
        /// 将插件文件复制到卷影副本目录
        /// </summary>
        /// <param name="pluginFilePath">Plugin file path</param>
        /// <param name="shadowCopyPlugFolder">Path to shadow copy folder</param>
        /// <returns>插件文件的卷影副本的文件路径</returns>
        private static string ShadowCopyFile(string pluginFilePath, string shadowCopyPlugFolder)
        {
            var shouldCopy = true;
            var shadowCopiedPlug = _fileProvider.Combine(shadowCopyPlugFolder, _fileProvider.GetFileName(pluginFilePath));

            // 检查阴影复制文件是否已存在，如果已存在，请检查是否已更新，是否不复制
            if (_fileProvider.FileExists(shadowCopiedPlug))
            {
                // 最好使用LastWriteTimeUTC，但并非所有文件系统都有这个属性也许比较文件哈希更好？
                var areFilesIdentical = _fileProvider.GetCreationTime(shadowCopiedPlug).ToUniversalTime().Ticks >= _fileProvider.GetCreationTime(pluginFilePath).ToUniversalTime().Ticks;
                if (areFilesIdentical)
                {
                    shouldCopy = false;
                }
                else
                {
                    // 删除现有文件
                    //More info: https://www.nopcommerce.com/boards/t/11511/access-error-nopplugindiscountrulesbillingcountrydll.aspx?p=4#60838
                    _fileProvider.DeleteFile(shadowCopiedPlug);
                }
            }

            if (!shouldCopy)
                return shadowCopiedPlug;

            try
            {
                _fileProvider.FileCopy(pluginFilePath, shadowCopiedPlug, true);
            }
            catch (IOException)
            {
                // 当文件被锁定时会出现这种情况，由于某种原因，devenv会锁定一些插件文件，
                // 而另一个疯狂的原因是你可以重命名它们释放锁，这样我们在这里做的就是重命名，我们可以 重影复制
                try
                {
                    var oldFile = shadowCopiedPlug + Guid.NewGuid().ToString("N") + ".old";
                    _fileProvider.FileMove(shadowCopiedPlug, oldFile);
                }
                catch (IOException exc)
                {
                    throw new IOException(shadowCopiedPlug + " rename failed, cannot initialize plugin", exc);
                }
                //OK, we've made it this far, now retry the shadow copy
                _fileProvider.FileCopy(pluginFilePath, shadowCopiedPlug, true);
            }

            return shadowCopiedPlug;
        }

        /// <summary>
        /// 确定文件夹是否为包的bin插件文件夹
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <returns>如果文件夹是包的bin插件文件夹，则为true；否则为false。</returns>
        private static bool IsPackagePluginFolder(string folder)
        {
            if (string.IsNullOrEmpty(folder))
                return false;

            var parent = _fileProvider.GetParentDirectory(folder);

            if (string.IsNullOrEmpty(parent))
                return false;

            if (!_fileProvider.GetDirectoryNameOnly(parent).Equals(NGPPluginDefaults.PathName, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="applicationPartManager">Application part manager</param>
        /// <param name="config">Config</param>
        public static void Initialize(ApplicationPartManager applicationPartManager)
        {
            if (applicationPartManager == null)
                throw new ArgumentNullException(nameof(applicationPartManager));

            using (new WriteLockDisposable(Locker))
            {
                // TODO: 在此添加详细的异常处理/引发，因为这是在应用程序启动时发生的，可能会阻止应用程序完全启动。
                var pluginFolder = _fileProvider.MapPath(NGPPluginDefaults.Path);
                _shadowCopyFolder = _fileProvider.MapPath(NGPPluginDefaults.ShadowCopyPath);
                _reserveShadowCopyFolder = _fileProvider.Combine(_fileProvider.MapPath(NGPPluginDefaults.ShadowCopyPath), $"{NGPPluginDefaults.ReserveShadowCopyPathName}{DateTime.Now.ToFileTimeUtc()}");

                var referencedPlugins = new List<PluginDescriptor>();

                try
                {
                    var installedPluginSystemNames = GetInstalledPluginNames(_fileProvider.MapPath(NGPPluginDefaults.InstalledPluginsFilePath));

                    // 确保创建文件夹
                    _fileProvider.CreateDirectory(pluginFolder);
                    _fileProvider.CreateDirectory(_shadowCopyFolder);

                    // 获取bin中所有文件的列表
                    var binFiles = _fileProvider.GetFiles(_shadowCopyFolder, "*", false);
                    foreach (var f in binFiles)
                    {
                        if (_fileProvider.GetFileName(f).Equals("placeholder.txt", StringComparison.InvariantCultureIgnoreCase))
                            continue;

                        try
                        {
                            //ignore index.htm
                            var fileName = _fileProvider.GetFileName(f);

                            _fileProvider.DeleteFile(f);
                        }
                        catch
                        {
                            // TODO:书写异常
                        }
                    }

                    // 删除所有保留文件夹
                    foreach (var directory in _fileProvider.GetDirectories(_shadowCopyFolder, NGPPluginDefaults.ReserveShadowCopyPathNamePattern))
                    {
                        try
                        {
                            _fileProvider.DeleteDirectory(directory);
                        }
                        catch
                        {
                            // TODO:书写异常
                        }
                    }

                    // 加载描述文件
                    foreach (var dfd in GetDescriptionFilesAndDescriptors(pluginFolder))
                    {
                        var descriptionFile = dfd.Key;
                        var pluginDescriptor = dfd.Value;

                        // 一些验证
                        if (string.IsNullOrWhiteSpace(pluginDescriptor.SystemName))
                            throw new Exception($"A plugin '{descriptionFile}' has no system name. Try assigning the plugin a unique name and recompiling.");
                        if (referencedPlugins.Contains(pluginDescriptor))
                            throw new Exception($"A plugin with '{pluginDescriptor.SystemName}' system name is already defined");

                        // 设定是否加载属性
                        pluginDescriptor.Installed = installedPluginSystemNames
                            .FirstOrDefault(x => x.Equals(pluginDescriptor.SystemName, StringComparison.InvariantCultureIgnoreCase)) != null;

                        try
                        {
                            var directoryName = _fileProvider.GetDirectoryName(descriptionFile);
                            if (string.IsNullOrEmpty(directoryName))
                                throw new Exception($"Directory cannot be resolved for '{_fileProvider.GetFileName(descriptionFile)}' description file");

                            // 获取插件中所有DLL的列表（不在bin中！）
                            var pluginFiles = _fileProvider.GetFiles(directoryName, "*.dll", false)
                                .Where(x => !binFiles.Select(q => q).Contains(x))
                                .Where(x => IsPackagePluginFolder(_fileProvider.GetDirectoryName(x)))
                                .ToList();

                            // 其他插件的描述信息
                            var mainPluginFile = pluginFiles
                                .FirstOrDefault(x => _fileProvider.GetFileName(x).Equals(pluginDescriptor.AssemblyFileName, StringComparison.InvariantCultureIgnoreCase));

                            // 插件目录错误
                            if (mainPluginFile == null)
                            {
                                continue;
                            }

                            pluginDescriptor.OriginalAssemblyFile = mainPluginFile;

                            // 实际加载程序集
                            pluginDescriptor.ReferencedAssembly = PerformFileDeploy(mainPluginFile, applicationPartManager);

                            // 加载所有其他引用的程序集
                            foreach (var plugin in pluginFiles
                                .Where(x => !_fileProvider.GetFileName(x).Equals(_fileProvider.GetFileName(mainPluginFile), StringComparison.InvariantCultureIgnoreCase))
                                .Where(x => !IsAlreadyLoaded(x)))
                                PerformFileDeploy(plugin, applicationPartManager);

                            // init插件类型（每个程序集只允许一个插件）
                            foreach (var t in pluginDescriptor.ReferencedAssembly.GetTypes())
                                if (typeof(IPlugin).IsAssignableFrom(t))
                                    if (!t.IsInterface)
                                        if (t.IsClass && !t.IsAbstract)
                                        {
                                            pluginDescriptor.PluginType = t;
                                            break;
                                        }

                            referencedPlugins.Add(pluginDescriptor);
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            // 添加插件名称。并从新抛出异常
                            var msg = $"Plugin '{pluginDescriptor.FriendlyName}'. ";
                            foreach (var e in ex.LoaderExceptions)
                                msg += e.Message + Environment.NewLine;

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                        catch (Exception ex)
                        {
                            // 添加插件名称。并从新抛出异常
                            var msg = $"Plugin '{pluginDescriptor.FriendlyName}'. {ex.Message}";

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    for (var e = ex; e != null; e = e.InnerException)
                        msg += e.Message + Environment.NewLine;

                    var fail = new Exception(msg, ex);
                    throw fail;
                }

                ReferencedPlugins = referencedPlugins;
            }
        }

        /// <summary>
        /// 将插件标记为已安装
        /// </summary>
        /// <param name="systemName">插件系统名称</param>
        public static void MarkPluginAsInstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException(nameof(systemName));

            var filePath = _fileProvider.MapPath(NGPPluginDefaults.InstalledPluginsFilePath);

            // 如果不存在，则创建文件
            _fileProvider.CreateFile(filePath);

            // 获取已安装的插件名称
            var installedPluginSystemNames = GetInstalledPluginNames(filePath);

            // 如果插件系统名称不存在，请将其添加到列表中
            var alreadyMarkedAsInstalled = installedPluginSystemNames.Any(pluginName => pluginName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
            if (!alreadyMarkedAsInstalled)
                installedPluginSystemNames.Add(systemName);

            // 将已安装的插件名称保存到文件中
            SaveInstalledPluginNames(installedPluginSystemNames, filePath);
        }

        /// <summary>
        /// 将插件标记为已卸载
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        public static void MarkPluginAsUninstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException(nameof(systemName));

            var filePath = _fileProvider.MapPath(NGPPluginDefaults.InstalledPluginsFilePath);

            // 如果不存在，则创建文件
            _fileProvider.CreateFile(filePath);

            // 获取已安装的插件名称
            var installedPluginSystemNames = GetInstalledPluginNames(filePath);

            // 从列表中删除插件系统名称（如果存在）
            var alreadyMarkedAsInstalled = installedPluginSystemNames.Any(pluginName => pluginName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
            if (alreadyMarkedAsInstalled)
                installedPluginSystemNames.Remove(systemName);

            // 将已安装的插件名称保存到文件中
            SaveInstalledPluginNames(installedPluginSystemNames, filePath);
        }

        /// <summary>
        /// 将插件标记为已卸载（全部标记）
        /// </summary>
        public static void MarkAllPluginsAsUninstalled()
        {
            var filePath = _fileProvider.MapPath(NGPPluginDefaults.InstalledPluginsFilePath);
            if (_fileProvider.FileExists(filePath))
                _fileProvider.DeleteFile(filePath);
        }

        /// <summary>
        /// 按与插件位于同一程序集中的某种类型查找插件描述符
        /// </summary>
        /// <param name="typeInAssembly">Type</param>
        /// <returns>插件描述符（如果存在）; 否则为null></returns>
        public static PluginDescriptor FindPlugin(Type typeInAssembly)
        {
            if (typeInAssembly == null)
                throw new ArgumentNullException(nameof(typeInAssembly));

            return ReferencedPlugins?.FirstOrDefault(plugin =>
                plugin.ReferencedAssembly != null &&
                plugin.ReferencedAssembly.FullName.Equals(typeInAssembly.Assembly.FullName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// 从插件描述文件中获取插件描述符
        /// </summary>
        /// <param name="filePath">描述文件的路径</param>
        /// <returns>插件描述</returns>
        public static PluginDescriptor GetPluginDescriptorFromFile(string filePath)
        {
            var text = _fileProvider.ReadAllText(filePath, Encoding.UTF8);

            return GetPluginDescriptorFromText(text);
        }

        /// <summary>
        /// 从描述文本中获取插件描述
        /// </summary>
        /// <param name="text">Description text</param>
        /// <returns>Plugin descriptor</returns>
        public static PluginDescriptor GetPluginDescriptorFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new PluginDescriptor();

            // 从JSON文件中获取插件描述
            var descriptor = JsonConvert.DeserializeObject<PluginDescriptor>(text);

            return descriptor;
        }

        /// <summary>
        /// 将插件描述保存到插件描述文件中
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor</param>
        public static void SavePluginDescriptor(PluginDescriptor pluginDescriptor)
        {
            if (pluginDescriptor == null)
                throw new ArgumentException(nameof(pluginDescriptor));

            // 获取描述文件路径
            if (pluginDescriptor.OriginalAssemblyFile == null)
                throw new Exception($"Cannot load original assembly path for {pluginDescriptor.SystemName} plugin.");

            var filePath = _fileProvider.Combine(_fileProvider.GetDirectoryName(pluginDescriptor.OriginalAssemblyFile), NGPPluginDefaults.DescriptionFileName);
            if (!_fileProvider.FileExists(filePath))
                throw new Exception($"Description file for {pluginDescriptor.SystemName} plugin does not exist. {filePath}");

            var text = JsonConvert.SerializeObject(pluginDescriptor, Formatting.Indented);
            _fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// 从磁盘存储中删除插件目录
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor</param>
        /// <returns>如果删除了插件目录，则为True，否则为false</returns>
        public static bool DeletePlugin(PluginDescriptor pluginDescriptor)
        {
            // 没有插件描述
            if (pluginDescriptor == null)
                return false;

            // 检查是否安装了插件
            if (pluginDescriptor.Installed)
                return false;

            var directoryName = _fileProvider.GetDirectoryName(pluginDescriptor.OriginalAssemblyFile);

            if (_fileProvider.DirectoryExists(directoryName))
                _fileProvider.DeleteDirectory(directoryName);

            return true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 返回已经加载的插件集合
        /// </summary>
        public static IEnumerable<PluginDescriptor> ReferencedPlugins { get; set; }
        #endregion
    }
}