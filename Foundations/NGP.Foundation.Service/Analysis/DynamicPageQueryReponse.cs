/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * DynamicPageQueryReponse Description:
 * 动态分页查询返回
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/1/22 10:29:45    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Runtime.Serialization;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态分页查询返回
    /// </summary>
    [DataContract]
    public class DynamicPageQueryReponse
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        [DataMember(Name = "totalCount")]
        public int TotalCount { get; set; }
        /// <summary>
        /// 数据列表
        /// </summary>
        [DataMember(Name = "Data")]
        public dynamic Data { get; set; }
    }
}
