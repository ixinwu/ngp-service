/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * OAuthUserInfo Description:
 * 认证用户对象
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-2-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;

namespace NGP.Foundation.Service.System
{
    /// <summary>
    /// 认证用户对象
    /// </summary>
    public class OAuthUserInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
