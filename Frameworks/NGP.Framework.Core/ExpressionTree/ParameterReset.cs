/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ParameterReset Description:
 * 表达式参数Format
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Linq.Expressions;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 表达式参数Format
    /// </summary>
    public class ParameterReset : ExpressionVisitor
    {
        /// <summary>
        /// 字典
        /// </summary>
        private Dictionary<ParameterExpression, ParameterExpression> Dictionary { get; set; }

        /// <summary>
        /// 参数重设
        /// </summary>
        /// <param name="dictionary"></param>
        public ParameterReset(Dictionary<ParameterExpression, ParameterExpression> dictionary)
        {
            this.Dictionary = dictionary ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// 替换参数列表
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Expression ReplaceParameterList(Dictionary<ParameterExpression, ParameterExpression> dictionary, Expression expression)
        {
            return new ParameterReset(dictionary).Visit(expression);
        }

        /// <summary>
        /// 重置参数值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression param)
        {
            ParameterExpression replaceParam;
            if (Dictionary.TryGetValue(param, out replaceParam))
            {
                param = replaceParam;
            }
            return base.VisitParameter(param);
        }
    }
}
