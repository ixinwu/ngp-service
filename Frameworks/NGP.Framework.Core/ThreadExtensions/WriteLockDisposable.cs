/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * WriteLockDisposable Description:
 * 读写锁using模式
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/
using System;
using System.Threading;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 读写锁using模式
    /// </summary>
    public class WriteLockDisposable : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteLockDisposable"/> class.
        /// </summary>
        /// <param name="rwLock">The rw lock.</param>
        public WriteLockDisposable(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
            _rwLock.EnterWriteLock();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>

        protected virtual void Dispose(bool disposing)
        {
            _rwLock.ExitWriteLock();
        }
    }
}
