/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * TransactionCallHandlerAttribute Description:
 * 事务特性拦截
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Transactions;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 事务特性拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TransactionCallHandlerAttribute : Attribute
    {
        /// <summary>
        /// 是否启用环境事务
        /// </summary>
        public bool IsTransactionScope { get; set; }

        /// <summary>
        /// 是否有上下文提交
        /// </summary>
        public bool IsContextCommit { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 事务范围
        /// </summary>
        public TransactionScopeOption ScopeOption { get; set; }

        /// <summary>
        /// 事务隔离级别
        /// </summary>
        public IsolationLevel IsolationLevel { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="isTransactionScope"></param>
        /// <param name="isContextCommit"></param>
        /// <param name="timeout"></param>
        /// <param name="scopeOption"></param>
        /// <param name="isolationLevel"></param>
        public TransactionCallHandlerAttribute(bool isTransactionScope = false,
            bool isContextCommit = true, 
            int timeout = 60,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            IsTransactionScope = isTransactionScope;
            IsContextCommit = isContextCommit;
            Timeout = timeout;
            ScopeOption = scopeOption;
            IsolationLevel = isolationLevel;
        }
    }
}
