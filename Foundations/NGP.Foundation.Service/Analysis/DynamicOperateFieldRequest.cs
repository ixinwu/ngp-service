/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicOperateFieldRequest Description:
 * 动态操作字段对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/1/22  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态详情操作字段对象
    /// </summary>
    public class DynamicOperateFieldRequest
    {
        /// <summary>
        /// 字段key
        /// </summary>
        public string FieldKey { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 原始值
        /// </summary>
        public object OriginalValue { get; set; }
    }
}
