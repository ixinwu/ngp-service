/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicDetailOperateInfo Description:
 * 动态详情页面操作对象
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
    /// 动态详情页面操作对象
    /// </summary>
    [DataContract]
    public class DynamicUpdateRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 主键值
        /// </summary>
        [DataMember(Name = "primaryKeyValue")]
        public string PrimaryKeyValue { get; set; }

        /// <summary>
        /// 操作字段列表
        /// </summary>
        [DataMember(Name = "operateFields")]
        public List<DynamicAddRequest> OperateFields { get; set; }
    }
}
