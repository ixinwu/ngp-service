/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IServiceCommand Description:
 * 
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2017/7/26 10:35:52    yulin@ixinwu.com
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
    /// 服务Command
    /// </summary>
    public interface IServiceCommand
    {
        /// <summary>
        /// 服务key
        /// </summary>
        string ServiceKey { get; }

        /// <summary>
        /// 当前执行的任务
        /// </summary>
        /// <param name="descriptor">服务项</param>
        void Excute(ServiceJobDescriptor descriptor);
    }
}
