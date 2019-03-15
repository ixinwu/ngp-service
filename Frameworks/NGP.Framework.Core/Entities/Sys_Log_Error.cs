/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * Class Description:
 * 
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/4 15:37:37   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// 系统异常日志
    /// </summary>
    public class Sys_Log_Error : BaseDBEntity
    {
        /// <summary>
        /// api路径
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// api提交参数
        /// </summary>
        public string Parameters { get; set; }
        /// <summary>
        /// 业务方法
        /// </summary>
        public string BusinessMethod { get; set; }
        /// <summary>
        /// 异常内容
        /// </summary>
        public string ExceptionContent { get; set; }
    }
}