/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IServiceContainer Description:
 * 
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2017/7/21 15:08:22    yulin@ixinwu.com
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
    /// 服务容器接口
    /// </summary>
    public interface IServiceContainer
    {
        /// <summary>
        /// 启动
        /// </summary>
        void Start();

        /// <summary>
        /// 停止
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 新增服务项
        /// </summary>
        /// <param name="item">服务项</param>
        void AddServiceJob(ServiceJobDescriptor item);

        /// <summary>
        /// 修改服务项触发器
        /// </summary>
        /// <param name="item">服务项</param>
        void ModifyServiceJobTrigger(ServiceJobDescriptor item);

        /// <summary>
        /// 去除服务项
        /// </summary>
        /// <param name="item">服务项</param>
        void RemoveServiceJob(ServiceJobDescriptor item);

        /// <summary>
        /// 服务项列表
        /// </summary>
        IEnumerable<ServiceJobDescriptor> ServiceJobDescriptors { get; }
    }
}
