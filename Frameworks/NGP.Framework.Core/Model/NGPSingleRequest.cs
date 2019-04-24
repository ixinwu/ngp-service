/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPSingleRequest Description:
 * ngp单对象请求
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Runtime.Serialization;

namespace NGP.Framework.Core
{
    /// <summary>
    /// ngp单对象请求
    /// </summary>
    [DataContract]
    public class NGPSingleRequest<T> : INGPRequest
    {
        /// <summary>
        /// 请求值
        /// </summary>
        [DataMember(Name = "requestData")]
        public T RequestData { get; set; }
    }

    /// <summary>
    /// ngp单对象请求
    /// </summary>
    [DataContract]
    public class NGPSingleRequest : NGPSingleRequest<string>
    {
    }
}
