/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * BasePlugin Description:
 * 插件基类
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 插件基类
    /// </summary>
    public abstract class BasePlugin : IPlugin
    {
        /// <summary>
        /// 获取插件描述
        /// </summary>
        public virtual PluginDescriptor PluginDescriptor { get; set; }

        /// <summary>
        /// 按照插件
        /// </summary>
        public virtual void Install() 
        {
            PluginManager.MarkPluginAsInstalled(PluginDescriptor.SystemName);
        }

        /// <summary>
        /// 卸载插件
        /// </summary>
        public virtual void Uninstall() 
        {
            PluginManager.MarkPluginAsUninstalled(PluginDescriptor.SystemName);
        }
    }
}
