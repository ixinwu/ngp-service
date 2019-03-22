/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * AppExtendConfig Description:
 * 应用扩展配置
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System.Collections.Generic;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 应用扩展配置
    /// </summary>
    public class AppExtendConfig
    {
        /// <summary>
        /// 显示字段key
        /// </summary>
        public bool? IsGlobal { get; set; }

        /// <summary>
        /// 默认字段列表
        /// </summary>
        public List<AppDefaultFieldConfig> DefaultFields { get; set; }
    }
}
