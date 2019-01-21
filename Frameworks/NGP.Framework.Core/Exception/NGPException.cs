/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * NGPException Description:
 * ngp异常
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Runtime.Serialization;

namespace NGP.Framework.Core
{
    /// <summary>
    /// ngp应用程序异常
    /// </summary>
    [Serializable]
    public class NGPException : Exception
    {
        /// <summary>
        /// ctor
        /// </summary>
        public NGPException()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        public NGPException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public NGPException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected NGPException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NGPException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
