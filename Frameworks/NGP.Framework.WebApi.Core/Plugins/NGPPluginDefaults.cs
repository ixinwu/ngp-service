/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPPluginDefaults Description:
 * 插件相关的默认值
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 插件相关的默认值
    /// </summary>
    public static partial class NGPPluginDefaults
    {
        /// <summary>
        /// 获取包含已安装的插件系统名称的文件的路径
        /// </summary>
        public static string InstalledPluginsFilePath => "~/App_Data/installedPlugins.json";

        /// <summary>
        /// 获取plugins文件夹的路径
        /// </summary>
        public static string Path => "~/Plugins";

        /// <summary>
        /// 获取插件文件夹名称
        /// </summary>
        public static string PathName => "Plugins";

        /// <summary>
        /// 获取插件影拷贝路径
        /// </summary>
        public static string ShadowCopyPath => "~/Plugins/bin";

        /// <summary>
        /// 获取plugins refs文件夹的路径
        /// </summary>
        public static string RefsPathName => "refs";

        /// <summary>
        /// 获取插件描述文件的名称
        /// </summary>
        public static string DescriptionFileName => "plugin.json";

        /// <summary>
        /// 获取影拷贝文件的路径
        /// </summary>
        public static string ReserveShadowCopyPathName => "reserve_bin_";

        /// <summary>
        /// 获取影拷贝文件的名称匹配模型
        /// </summary>
        public static string ReserveShadowCopyPathNamePattern => "reserve_bin_*";
    }
}