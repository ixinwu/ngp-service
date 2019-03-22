/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * OperatorResolveBuildExcuteStep Description:
 * 操作解析执行步骤
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/18 15:56:08    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Foundation.Resources;
using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 操作解析执行步骤
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class OperatorResolveBuildExcuteStep<TRequest> : StepBase<OperatorResolveContext<TRequest>> where TRequest : DynamicBaseRequest
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(OperatorResolveContext<TRequest> ctx)
        {
            var engine = Singleton<IEngine>.Instance.Resolve<ILinqParserHandler>();
            var unitRepository = Singleton<IEngine>.Instance.Resolve<IUnitRepository>();
            var workContext = Singleton<IEngine>.Instance.Resolve<IWorkContext>();

            var parserResult = engine.Resolve(new LinqParserRequest
            {
                Current = workContext.Current,
                DslContent = ctx.ExcuteLinqText
            });

            var count = unitRepository.ExecuteNonQuery(parserResult.Command.CommandText, parserResult.Command.ParameterCollection);
            if (count == 0)
            {
                ctx.Response = new NGPResponse
                {
                    AffectedRows = 0,
                    ErrorCode = ErrorCode.SystemError,
                    Message = CommonResource.OperatorError,
                    Status = OperateStatus.Error
                };
                return false;
            }

            ctx.Response = new NGPResponse
            {
                AffectedRows = count,
                Message = CommonResource.OperatorSuccess,
                Status = OperateStatus.Success
            };
            return true;
        }


    }
}
