/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * AppDomainTypeFinder Description:
 * 类型查找实现
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 通过循环当前执行的AppDomain中的程序集来查找NGP所需类型的类。
    /// 只调查名称与某些模式匹配的程序集，
    /// 并且始终调查由assiable引用的程序集的可选列表。
    /// </summary>
    public class NGPTypeFinder : ITypeFinder
    {
        #region Fields
        /// <summary>
        /// file provider
        /// </summary>
        protected INGPFileProvider _fileProvider;

        private bool _ignoreReflectionErrors = true;

        private bool _binFolderAssembliesLoaded;

        #endregion

        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileProvider"></param>
        public NGPTypeFinder(INGPFileProvider fileProvider = null)
        {
            this._fileProvider = fileProvider ?? CommonHelper.DefaultFileProvider;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 迭代AppDomain中的所有程序集，如果其名称与配置的模式匹配，则将其添加到列表中。
        /// </summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!Matches(assembly.FullName))
                    continue;

                if (addedAssemblyNames.Contains(assembly.FullName))
                    continue;

                assemblies.Add(assembly);
                addedAssemblyNames.Add(assembly.FullName);
            }
        }

        /// <summary>
        /// 添加特定配置的程序集。
        /// </summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        protected virtual void AddConfiguredAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (var assemblyName in AssemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                if (addedAssemblyNames.Contains(assembly.FullName))
                    continue;

                assemblies.Add(assembly);
                addedAssemblyNames.Add(assembly.FullName);
            }
        }

        /// <summary>
        /// 检查DLL是否是我们知道不需要调查的已发送的DLL之一
        /// </summary>
        /// <param name="assemblyFullName">
        /// 要检查的程序集的名称。
        /// </param>
        /// <returns>
        /// 如果程序集应加载到NGP中，则为true。
        /// </returns>
        public virtual bool Matches(string assemblyFullName)
        {
            return !Matches(assemblyFullName, AssemblySkipLoadingPattern)
                   && Matches(assemblyFullName, AssemblyRestrictToLoadingPattern);
        }

        /// <summary>
        /// 检查DLL是否是我们知道不需要调查的已发送的DLL之一
        /// </summary>
        /// <param name="assemblyFullName">
        /// 要检查的程序集的名称
        /// </param>
        /// <param name="pattern">
        /// 要与程序集名称匹配的正则表达式模式。
        /// </param>
        /// <returns>
        /// 如果程序集应加载到NGP中，则为true。
        /// </returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// 确保所提供文件夹中的匹配程序集已加载到应用程序域中。
        /// </summary>
        /// <param name="directoryPath">
        /// 包含要在应用程序域中加载的DLL的目录的物理路径。
        /// </param>
        protected virtual void LoadMatchingAssemblies(string directoryPath)
        {
            var loadedAssemblyNames = new List<string>();

            foreach (var a in GetAssemblies())
            {
                loadedAssemblyNames.Add(a.FullName);
            }

            if (!_fileProvider.DirectoryExists(directoryPath))
            {
                return;
            }

            foreach (var dllPath in _fileProvider.GetFiles(directoryPath, "*.dll"))
            {
                try
                {
                    var an = AssemblyName.GetAssemblyName(dllPath);
                    if (Matches(an.FullName) && !loadedAssemblyNames.Contains(an.FullName))
                    {
                        App.Load(an);
                    }
                }
                catch (BadImageFormatException ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 类型是否实现泛型？
        /// </summary>
        /// <param name="type"></param>
        /// <param name="openGeneric"></param>
        /// <returns></returns>
        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
                {
                    if (!implementedInterface.IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取与当前实现相关的程序集。
        /// </summary>
        /// <returns>程序集列表</returns>
        protected virtual IList<Assembly> GetCurrentAssemblies()
        {
            var addedAssemblyNames = new List<string>();
            var assemblies = new List<Assembly>();

            if (LoadAppDomainAssemblies)
                AddAssembliesInAppDomain(addedAssemblyNames, assemblies);
            AddConfiguredAssemblies(addedAssemblyNames, assemblies);

            return assemblies;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 查找类型
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="onlyConcreteClasses">指示是否仅查找具体类的值</param>
        /// <returns>Result</returns>
        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), onlyConcreteClasses);
        }

        /// <summary>
        /// 查找类型
        /// </summary>
        /// <param name="assignTypeFrom">Assign type from</param>
        /// <param name="onlyConcreteClasses">指示是否仅查找具体类的值</param>
        /// <returns>Result</returns>
        /// <returns></returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
        }

        /// <summary>
        /// 查找类型
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">指示是否仅查找具体类的值</param>
        /// <returns>Result</returns>
        public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);
        }

        /// <summary>
        /// 查找类型
        /// </summary>
        /// <param name="assignTypeFrom">Assign type from</param>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">指示是否仅查找具体类的值</param>
        /// <returns>Result</returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();
            try
            {
                foreach (var a in assemblies)
                {
                    Type[] types = null;
                    try
                    {
                        types = a.GetTypes();
                    }
                    catch
                    {
                        //Entity Framework 6 doesn't allow getting types (throws an exception)
                        if (!_ignoreReflectionErrors)
                        {
                            throw;
                        }
                    }

                    if (types == null)
                        continue;

                    foreach (var t in types)
                    {
                        if (!assignTypeFrom.IsAssignableFrom(t) && (!assignTypeFrom.IsGenericTypeDefinition || !DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                            continue;

                        if (t.IsInterface)
                            continue;

                        if (onlyConcreteClasses)
                        {
                            if (t.IsClass && !t.IsAbstract)
                            {
                                result.Add(t);
                            }
                        }
                        else
                        {
                            result.Add(t);
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = string.Empty;
                foreach (var e in ex.LoaderExceptions)
                    msg += e.Message + Environment.NewLine;

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }

            return result;
        }

        /// <summary>
        /// 获取与当前实现相关的程序集。
        /// </summary>
        /// <returns>程序集列表</returns>
        public virtual IList<Assembly> GetAssemblies()
        {

            if (!EnsureBinFolderAssembliesLoaded || _binFolderAssembliesLoaded)
                return GetCurrentAssemblies();

            _binFolderAssembliesLoaded = true;
            var binPath = GetBinDirectory();
            LoadMatchingAssemblies(binPath);

            return GetCurrentAssemblies();
        }

        /// <summary>
        /// 获取\ Bin目录的物理磁盘路径
        /// </summary>
        /// <returns>物理路径. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string GetBinDirectory()
        {
            return AppContext.BaseDirectory;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 要在其中查找类型的应用程序域。
        /// </summary>
        public virtual AppDomain App => AppDomain.CurrentDomain;

        /// <summary>
        /// 获取或设置加载NGP类型时NGP是否应在应用程序域中迭代程序集。加载这些程序集时应用加载模式。
        /// </summary>
        public bool LoadAppDomainAssemblies { get; set; } = true;

        /// <summary>
        /// 获取或设置除了在AppDomain中加载的程序集外，还加载了启动程序集。
        /// </summary>
        public IList<string> AssemblyNames { get; set; } = new List<string>();

        /// <summary>
        /// 获取我们知道不需要加载的DLL列表。
        /// </summary>
        public string AssemblySkipLoadingPattern { get; set; } = "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";

        /// <summary>获取或设置将要调查的dll的模式。为了便于使用，除了提高性能之外，此默认值将匹配所有项，您可能需要配置一个包含程序集和您自己的模式。</summary>
        /// <remarks>如果更改此项以使NGP程序集不受调查（例如，不包含类似于“^NGP”……”的内容），则可能会破坏核心功能。</remarks>
        public string AssemblyRestrictToLoadingPattern { get; set; } = ".*";

        /// <summary>
        /// 获取或设置是否应特别检查Web应用程序的bin文件夹中的程序集是否在应用程序加载时加载。 
        /// 在重新加载应用程序后需要在AppDomain中加载插件的情况下需要这样做。
        /// </summary>
        public bool EnsureBinFolderAssembliesLoaded { get; set; } = true;

        #endregion
    }
}
