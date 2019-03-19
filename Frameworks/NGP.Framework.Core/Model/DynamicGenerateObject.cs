/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * DynamicGenerateObject Description:
 * 生成动态对象模型
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/1/23 13:48:53 hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 生成动态对象模型
    /// </summary>
    public class DynamicGenerateObject
    {
        /// <summary>
        /// 对象key
        /// </summary>
        public string ObjectKey { get; set; }

        /// <summary>
        /// 字段代码类型
        /// </summary>
        public Type CodeType { get; set; }
    }
}
