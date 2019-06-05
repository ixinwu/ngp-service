/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * OperatorResolveBuildAddCommandStep Description:
 * 操作解析构建追加command步骤
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
    /// 操作解析构建追加command步骤
    /// </summary>
    public class OperatorResolveBuildInsertStep : StepBase<OperatorResolveContext<DynamicInsertRequest>>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(OperatorResolveContext<DynamicInsertRequest> ctx)
        {
            var defaultFields = new List<AppDefaultFieldConfig>();

            if (ctx.InitContext.App != null
               && ctx.InitContext.App.ExtendConfig != null
               && !ctx.InitContext.App.ExtendConfig.DefaultFields.IsNullOrEmpty())
            {
                defaultFields = ctx.InitContext.App.ExtendConfig.DefaultFields
                 .Where(s => s.DefaultType.Contains(AppDefaultFieldType.Insert.ToString("G")))
                 .ToList();
            }

            // 获取解析对象
            var parserCommand = Singleton<IEngine>.Instance.Resolve<ILinqParserCommand>();

            var commandList = new List<string>();

            var buildReulst = ResolveExtend.BuildInsertCommand(ctx.Request.OperateFields,
                ctx.InitContext, defaultFields);
            if (buildReulst.Response != null)
            {
                ctx.Response = buildReulst.Response;
                return false;
            }
            commandList.AddRange(buildReulst.CommandList);
            ctx.InsertPrimaryKeys.Add(buildReulst.PrimaryKeys);

            ctx.ExcuteLinqText = parserCommand.JoinInsert(commandList);
            return true;

        }
    }
}
