/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * KeyValuePairInfo Description:
 * 键值对模型
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
    /// 键值对模型
    /// </summary>
    [DataContract]
    public class NGPKeyValuePair<TKey, TValue>
    {
        /// <summary>
        /// 键
        /// </summary>
        [DataMember(Name = "key")]
        public TKey Key { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember(Name = "value")]
        public TValue Value { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember(Name = "orderIndex")]
        public int? OrderIndex { get; set; }
    }

    /// <summary>
    /// 键值对模型
    /// </summary>
    [DataContract]
    public class NGPKeyValuePair : NGPKeyValuePair<string, string>
    {
    }

    /// <summary>
    /// 键值对模型
    /// </summary>
    [DataContract]
    public class NGPKeyValuePair<TValue> : NGPKeyValuePair<string, TValue>
    {
    }
}
