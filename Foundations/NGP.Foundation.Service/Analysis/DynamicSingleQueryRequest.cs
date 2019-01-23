/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * DynamicSingleQueryRequest Description:
 * 动态单条查询请求
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/1/22 16:53:09    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System.Runtime.Serialization;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态单条查询请求
    /// </summary>
    [DataContract]
    public class DynamicSingleQueryRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 主键值
        /// </summary>
        [DataMember(Name = "primaryKeyValue")]
        public string PrimaryKeyValue { get; set; }
    }
}
