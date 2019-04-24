/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicDeleteRequest Description:
 * 动态删除请求对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态删除请求对象
    /// </summary>
    [DataContract]
    public class DynamicDeleteRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 查询表达式
        /// </summary>
        [DataMember(Name = "whereExpressions")]
        public List<string> WhereExpressions { get; set; }
    }
}
