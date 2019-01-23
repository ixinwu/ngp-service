/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * TokenRequest Description:
 * 认证用户对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Runtime.Serialization;

namespace NGP.Foundation.Service.Authentication
{
    /// <summary>
    /// 认证用户对象
    /// </summary>
    [DataContract]
    public class TokenRequest : INGPRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember(Name = "loginName")]
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
