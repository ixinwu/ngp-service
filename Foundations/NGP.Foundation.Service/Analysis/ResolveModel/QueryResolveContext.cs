/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved.
 * 
 * QueryResolveContext Description:
 * 查询解析上下文
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/2/19  hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 查询解析上下文
    /// </summary>
    public class QueryResolveContext
    {
        /// <summary>
        /// 请求对象
        /// </summary>
        public DynamicQueryRequest Request { get; set; }

        /// <summary>
        /// 分页查询请求
        /// </summary>
        public NGPPageQueryRequest<DynamicQueryRequest> PageQueryRequest { get; set; }

        /// <summary>
        /// 命令上下文
        /// </summary>
        public QueryResolveCommandContext CommandContext { get; set; } = new QueryResolveCommandContext();

        /// <summary>
        /// 初始化对象
        /// </summary>
        public ResolveInitContext InitContext { get; set; } = new ResolveInitContext();

        /// <summary>
        /// 生成上下文
        /// </summary>
        public QueryGenerateContext GenerateContext { get; set; } = new QueryGenerateContext();

        /// <summary>
        /// 关联上下文
        /// </summary>
        public QueryResolveAssociatedContext AssociatedContext { get; set; } = new QueryResolveAssociatedContext();

        /// <summary>
        /// 返回结果
        /// </summary>
        public NGPPageQueryResponse Response { get; set; } = new NGPPageQueryResponse() { Data = null };
    }
}
