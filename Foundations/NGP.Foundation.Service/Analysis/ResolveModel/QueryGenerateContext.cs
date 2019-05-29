/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * QueryGenerateContext Description:
 * 解析生成对象上下文
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 解析生成对象上下文
    /// </summary>
    public class QueryGenerateContext
    {
        /// <summary>
        /// 生成名称字段的列表
        /// </summary>
        public List<App_Config_FormField> GenerateNameFields { get; set; } = new List<App_Config_FormField>();
    }
}
