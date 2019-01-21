/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * BusinessLogInfo Description:
 * 业务日志
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 业务日志
    /// </summary>
    public class BusinessLogInfo
    {
        /// <summary>
        /// api路径
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// api提交参数
        /// </summary>
        public string ApiPostParameter { get; set; }

        /// <summary>
        /// 业务方法
        /// </summary>
        public string BusinessMethod { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperateBy { get; set; }

        /// <summary>
        /// 操作部门
        /// </summary>
        public string OperateDept { get; set; }       

        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperateType { get; set; }

        /// <summary>
        /// 日志详情
        /// </summary>
        public List<BusinessLogInfo> LogInfos { get; set; } 
    }
}
