/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * GenerateFieldInfo Description:
 * 生成动态字段模型
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/1/23 13:48:53 hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;

namespace NGP.Foundation.Service
{
    /// <summary>
    /// 生成动态字段模型
    /// </summary>
    public class DynamicGenerateFieldInfo
    {
        /// <summary>
        /// 字段key
        /// </summary>
        public string FieldKey { get; set; }

        /// <summary>
        /// 字段代码类型
        /// </summary>
        public Type CodeType { get; set; }

        /// <summary>
        /// 序列化标签
        /// </summary>
        public string SerializedLabel { get; set; }
    }
}
