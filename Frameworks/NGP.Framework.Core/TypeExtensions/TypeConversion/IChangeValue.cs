/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IChangeValue Description:
 * 类型转换接口定义
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 类型转换接口定义
    /// </summary>
    internal interface IChangeValue
    {

    }

    /// <summary>
    /// 类型转换接口定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IChangeValue<T> : IChangeValue
        where T : struct
    {
        /// <summary>
        /// 判断并且转换
        /// </summary>
        /// <param name="value">被转换值</param>
        /// <returns>转换结果对象</returns>
        ChangeValueInfo<T> CheckAndChange(string value);
    }
}
