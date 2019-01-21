/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * WebAppTypeFinder Description:
 * 提供有关当前Web应用程序中的类型的信息
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 提供有关当前Web应用程序中的类型的信息。
    /// </summary>
    public class WebAppTypeFinder : AppDomainTypeFinder
    {
        #region Fields

        private bool _binFolderAssembliesLoaded;

        #endregion

        #region Ctor

        public WebAppTypeFinder(INGPFileProvider fileProvider = null) : base(fileProvider)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置是否应特别检查Web应用程序的bin文件夹中的程序集是否在应用程序加载时加载。 
        /// 在重新加载应用程序后需要在AppDomain中加载插件的情况下需要这样做。
        /// </summary>
        public bool EnsureBinFolderAssembliesLoaded { get; set; } = true;

        #endregion

        #region Methods

        /// <summary>
        /// 获取\ Bin目录的物理磁盘路径
        /// </summary>
        /// <returns>物理路径. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string GetBinDirectory()
        {
            return AppContext.BaseDirectory;
        }

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <returns>Result</returns>
        public override IList<Assembly> GetAssemblies()
        {
            if (!EnsureBinFolderAssembliesLoaded || _binFolderAssembliesLoaded) 
                return base.GetAssemblies();

            _binFolderAssembliesLoaded = true;
            var binPath = GetBinDirectory();
            LoadMatchingAssemblies(binPath);

            return base.GetAssemblies();
        }

        #endregion
    }
}
