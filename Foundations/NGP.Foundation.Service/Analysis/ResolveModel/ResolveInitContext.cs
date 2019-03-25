/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * ResolveContext Description:
 * 解析上下文
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 解析上下文
    /// </summary>
    public class ResolveInitContext
    {
        /// <summary>
        /// 应用基本配置
        /// </summary>
        public App_Config_BaseInfo App { get; set; }

        /// <summary>
        /// data set对象
        /// </summary>
        public App_Config_DataSet DataSet { get; set; }

        /// <summary>
        /// 表单列表对象
        /// </summary>
        public List<App_Config_Form> Forms { get; set; }

        /// <summary>
        /// 表单关系对象
        /// </summary>
        public List<App_Config_FormRelation> FormRelations { get; set; }

        /// <summary>
        /// 表单字段对象
        /// </summary>
        public List<App_Config_FormField> FormFields { get; set; }
    }
}
