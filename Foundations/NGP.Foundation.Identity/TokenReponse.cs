/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * TokenReponse Description:
 * 登录对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System.Runtime.Serialization;

namespace NGP.Foundation.Identity
{
    /// <summary>
    /// 登录对象
    /// </summary>
    [DataContract]
    public class TokenReponse
    {
        /// <summary>
        /// 访问token
        /// </summary>
        [DataMember(Name = "accessToken")]
        public string AccessToken { get; set; }

        /// <summary>
        /// token类型
        /// </summary>
        [DataMember(Name = "tokenType")]
        public string TokenType { get; set; }
    }
}
