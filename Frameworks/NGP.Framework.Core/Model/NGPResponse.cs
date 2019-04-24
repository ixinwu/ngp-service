/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * OperateResultInfo Description:
 * 操作结果返回
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
    /// 操作结果返回
    /// </summary>
    [DataContract]
    public class NGPResponse : INGPResponse
    {
        /// <summary>
        /// 影响行数
        /// </summary>
        [DataMember(Name = "affectedRows")]
        public int AffectedRows { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        [DataMember(Name = "status")]
        public OperateStatus Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        [DataMember(Name = "errorCode")]
        public ErrorCode ErrorCode { get; set; }
    }

    /// <summary>
    /// 查询操作结果返回
    /// </summary>
    [DataContract]
    public class NGPResponse<T> : NGPResponse
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        [DataMember(Name = "data")]
        public T Data { get; set; }
    }
}
