/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicInsertRequest Description:
 * 动态追加请求对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2017/3/14  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态追加请求对象
    /// </summary>
    [DataContract]
    public class DynamicBulkInsertRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 操作字段列表组合
        /// </summary>
        [DataMember(Name = "operateFields")]
        public List<List<DynamicOperateFieldRequest>> OperateFields { get; set; }
    }
}
