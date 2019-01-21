/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IPlugin Description:
 * 插件接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 插件描述
        /// </summary>
        PluginDescriptor PluginDescriptor { get; set; }

        /// <summary>
        /// 安装插件
        /// </summary>
        void Install();

        /// <summary>
        /// 卸载插件
        /// </summary>
        void Uninstall();
    }
}
