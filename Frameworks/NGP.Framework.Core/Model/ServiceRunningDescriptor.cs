/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ServiceRunningDescriptor Description:
 * 
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/2/26 15:02:55    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 服务运行描述
    /// </summary>
    public class ServiceRunningDescriptor
    {
        /// <summary>
        /// Cron表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 有效起始时间
        /// </summary>
        public DateTime? ValidStartTime { get; set; }

        /// <summary>
        /// 有效结束时间
        /// </summary>
        public DateTime? ValidEndTime { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderIndex { get; set; }
    }
}
