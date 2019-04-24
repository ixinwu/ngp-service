/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPPageQueryReponse Description:
 * ngp分页查询返回
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/1/22 10:29:45    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NGP.Framework.Core
{
    /// <summary>
    /// ngp分页查询返回
    /// </summary>
    [DataContract]
    public class NGPPageQueryResponse
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        [DataMember(Name = "totalCount")]
        public int TotalCount { get; set; }
        /// <summary>
        /// 数据列表
        /// </summary>
        [DataMember(Name = "data")]
        public dynamic Data { get; set; }
    }

    /// <summary>
    /// ngp分页查询返回
    /// </summary>
    public class NGPPageQueryResponse<T> : NGPPageQueryResponse
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        [DataMember(Name = "data")]
        public new List<T> Data { get; set; }
    }
}
