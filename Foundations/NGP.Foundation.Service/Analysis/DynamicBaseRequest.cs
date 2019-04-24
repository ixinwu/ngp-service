/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * DynamicBaseRequest Description:
 * 动态请求基类
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Runtime.Serialization;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态请求基类
    /// </summary>
    [DataContract]
    public abstract class DynamicBaseRequest : INGPRequest
    {
        /// <summary>
        /// data set key
        /// </summary>
        [DataMember(Name = "dataSetKey")]
        public string DataSetKey { get; set; }

        /// <summary>
        /// 资源key(用于鉴权)
        /// </summary>
        [DataMember(Name = "resourceKey")]
        public string ResourceKey { get; set; }
    }
}
