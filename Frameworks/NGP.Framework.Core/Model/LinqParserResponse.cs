/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * LinqParserResponse Description:
 * linq解析返回对象
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
    /// linq解析返回对象
    /// </summary>
    public class LinqParserResponse
    {
        /// <summary>
        /// 解析结果command
        /// </summary>
        public ExcuteSqlCommand Command { get; set; }

        /// <summary>
        /// 生成对象列表
        /// </summary>
        public List<DynamicGenerateObject> GenerateObjects { get; set; }
    }
}
