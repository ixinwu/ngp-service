/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * DynamicLinqRequest Description:
 * 动态linq请求
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/1/22 16:53:09    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态linq请求
    /// </summary>
    [DataContract]
    public class DynamicLinqRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 执行dsl
        /// </summary>
        [DataMember(Name = "dsl")]
        public string Dsl { get; set; }
    }
}
