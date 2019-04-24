/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPStartEnd Description:
 * 开始结束对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2017-2-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Runtime.Serialization;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 开始结束对象
    /// </summary>
    [DataContract]
    public class NGPStartEnd<T>
    {
        /// <summary>
        /// 开始
        /// </summary>
        [DataMember(Name = "start")]
        public T Start { get; set; }

        /// <summary>
        /// 结束
        /// </summary>
        [DataMember(Name = "end")]
        public T End { get; set; }
    }

    /// <summary>
    /// 开始结束对象
    /// </summary>
    [DataContract]
    public class NGPStartEnd : NGPStartEnd<DateTime?>
    {
    }
}
