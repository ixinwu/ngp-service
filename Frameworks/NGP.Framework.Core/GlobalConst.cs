/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * GlobalConst Description:
 * 全局常量
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2017/3/21 17:04:04   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


namespace NGP.Framework.Core
{
    /// <summary>
    /// 全局常量
    /// </summary>
    public struct GlobalConst
    {
        /// <summary>
        /// 附件路径
        /// </summary>
        public const string __AttachmentFilesPath = "~/AttachmentFiles";

        /// <summary>
        /// 日期格式常量
        /// </summary>
        public struct DateFormatConst
        {
            /// <summary>
            /// 日期格式
            /// </summary>
            public const string __DateFormat = "yyyy-MM-dd";

            /// <summary>
            /// 时间格式
            /// </summary>
            public const string __DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// 系统人员常量
        /// </summary>
        public struct SysEmployeeConst
        {
            /// <summary>
            /// 人员id
            /// </summary>
            public const string __EmpId = "100001";

            /// <summary>
            /// 部门id
            /// </summary>
            public const string __DeptId = "0";
        }

        /// <summary>
        /// 策略数据映射key
        /// </summary>
        public const string __ServiceDataMapKey = "ServiceDataMapKey";

        /// <summary>
        /// 消息路由常量
        /// </summary>
        public struct MessageRouteConst
        {
            /// <summary>
            /// 数据处理路由
            /// </summary>
            public const string __DataProcessRoute = "DataProcessExchange-DataProcessQueue-DataProcessRoute";
        }
    }
}
