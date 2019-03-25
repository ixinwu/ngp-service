/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * LinqParserType Description:
 * linq解析类型
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/3/11 14:53:01 hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// linq解析类型
    /// </summary>
    public enum LinqParserType
    {
        /// <summary>
        /// 不处理的解析
        /// </summary>
        None = 0,

        /// <summary>
        /// 查询
        /// </summary>
        Query,

        /// <summary>
        /// 执行数据库操作
        /// </summary>
        ExecuteNonQuery,
    }
}
