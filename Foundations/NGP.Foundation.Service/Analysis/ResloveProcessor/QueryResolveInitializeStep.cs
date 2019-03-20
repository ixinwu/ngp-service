/* ------------------------------------------------------------------------------
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ResolveInitializeStep Description:
 * 解析初始化步骤
 *
 * Comment 					        Revision	Date                  Author
 * -----------------------------    --------    ------------------    ----------------
 * Created							1.0		    2019/3/18 15:56:08    hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 页面解析初始化处理
    /// </summary>
    public class QueryResolveInitializeStep : StepBase<QueryResloveContext>
    {
        /// <summary>
        /// 执行上下文
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool Process(QueryResloveContext ctx)
        {
            // 配置信息读取服务
            var dataProvider = Singleton<IEngine>.Instance.Resolve<IResloveDataProvider>();

            // 处理参数上下文
            ctx.InitContext = dataProvider.InitResloveContext(ctx.Request);

            // 分析当前解析的主表
            var formKeys = ctx.Request.QueryFieldKeys.Select(s => AppConfigExtend.GetFormKey(s)).Distinct();
            var findFormKeys = formKeys.ToList();
            var mainKey = string.Empty;
            foreach (var item in formKeys)
            {
                var sourceKey = FindSourceKey(item, ctx.InitContext.FormRelations, findFormKeys);
                if (findFormKeys.Contains(sourceKey))
                {
                    mainKey = sourceKey;
                    break;
                }
            }

            ctx.InitContext.MainFormKey = mainKey;
            return true;
        }

        /// <summary>
        /// find source key
        /// </summary>
        /// <param name="mainKey"></param>
        /// <param name="relations"></param>
        /// <param name="formKeys"></param>
        /// <returns></returns>
        private string FindSourceKey(string mainKey, List<App_Config_FormRelation> relations, List<string> formKeys)
        {
            // 取第一个源
            var sourceKey = relations
                .Where(s => s.RelationFormKey == mainKey && formKeys.Contains(s.SourceFormKey))
                .Select(s => s.SourceFormKey)
                .Distinct()
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(sourceKey))
            {
                return mainKey;
            }
            formKeys.Remove(mainKey);
            return FindSourceKey(sourceKey, relations, formKeys);
        }
    }
}
