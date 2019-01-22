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


namespace NGP.Foundation.Service.System
{
    /// <summary>
    /// 登录对象
    /// </summary>
    public class TokenReponse
    {
        /// <summary>
        /// 访问token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// token类型
        /// </summary>
        public string TokenType { get; set; }
    }
}
