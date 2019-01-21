/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ChangeValueInfo Description:
 * 类型转换模型定义
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// 类型转换模型定义
    /// </summary>
    /// <typeparam name="T">被转换类型</typeparam>
    internal class ChangeValueInfo<T> where T : struct
    {
        /// <summary>
        /// 转换结果
        /// </summary>
        public T ChangeResult { get; set; }

        /// <summary>
        /// 是否可以转换
        /// </summary>
        public bool HasChange { get; set; }
    }
}
