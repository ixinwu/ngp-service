/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ILogPublisher Description:
 * 日志发布者接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 日志发布者接口
    /// </summary>
    public interface ILogPublisher
    {
        /// <summary>
        /// 注册错误日志
        /// </summary>
        /// <param name="info">日志对象</param>
        void RegisterError(ErrorLogInfo info);

        /// <summary>
        /// 注册业务日志
        /// </summary>
        /// <param name="info">日志对象</param>
        void RegisterBusiness(BusinessLogContext info);
    }
}
