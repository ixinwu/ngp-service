/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicAddRequest Description:
 * 动态追求请求对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/1/22  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态追求请求对象
    /// </summary>
    [DataContract]
    public class DynamicAddRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 操作字段列表
        /// </summary>
        [DataMember(Name = "operateFields")]
        public List<DynamicOperateFieldRequest> OperateFields { get; set; }
    }
}
