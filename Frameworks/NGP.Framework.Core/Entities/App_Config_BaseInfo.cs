/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * App_Config_BaseInfo Description:
 * 应用基本配置表
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/7 14:49:49   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 应用基本配置表
    /// </summary>
    public class App_Config_BaseInfo : BaseDBEntity
    {
        /// <summary>
        /// 应用key
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 应用短名
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// 扩展配置
        /// </summary>
        public AppExtendConfig ExtendConfig { get; set; }
    }
}