/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * LinqParserRequest Description:
 * linq解析请求对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/



namespace NGP.Framework.Core
{
    /// <summary>
    /// linq解析请求对象
    /// </summary>
    
    public class LinqParserRequest
    {
        /// <summary>
        /// 工作人员信息
        /// </summary>
        public WorkEmployee Current { get; set; }

        /// <summary>
        /// dsl内容
        /// </summary>
        public string DslContent { get; set; }
    }
}
