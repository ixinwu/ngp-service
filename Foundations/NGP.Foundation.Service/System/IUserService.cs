/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IUserService Description:
 * 用户业务服务
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-20   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using NGP.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NGP.Foundation.Service.System
{
    /// <summary>
    /// 用户业务服务
    /// </summary>    
    public interface IUserService
    {
        TokenResultInfo Certification(OAuthUserInfo userInfo);
    }
}
