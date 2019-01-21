/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ChangeValue Description:
 * 类型转换
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 类型转换
    /// </summary>
    internal class BooleanChangeValue : IChangeValue<bool>
    {
        #region IChangeValue 成员

        public ChangeValueInfo<bool> CheckAndChange(string value)
        {
            bool changeValue = default(bool);
            var hasChange = bool.TryParse(value, out changeValue);

            return new ChangeValueInfo<bool>()
            {
                HasChange = hasChange,
                ChangeResult = changeValue
            };
        }

        #endregion
    }

    /// <summary>
    /// byte类型转换定义
    /// </summary>
    internal class ByteChangeValue : IChangeValue<byte>
    {
        #region IChangeValue 成员

        public ChangeValueInfo<byte> CheckAndChange(string value)
        {
            byte changeValue = default(byte);
            var hasChange = byte.TryParse(value, out changeValue);

            return new ChangeValueInfo<byte>()
            {
                HasChange = hasChange,
                ChangeResult = changeValue
            };
        }

        #endregion
    }

    /// <summary>
    /// datetime类型转换接口定义
    /// </summary>
    internal class DateTimeChangeValue : IChangeValue<DateTime>
    {
        #region IChangeValue 成员

        public ChangeValueInfo<DateTime> CheckAndChange(string value)
        {
            DateTime changeValue = default(DateTime);
            var hasChange = DateTime.TryParse(value, out changeValue);

            return new ChangeValueInfo<DateTime>()
            {
                HasChange = hasChange,
                ChangeResult = changeValue
            };
        }

        #endregion
    }

    /// <summary>
    /// decimal类型转换接口定义
    /// </summary>
    internal class DecimalChangeValue : IChangeValue<decimal>
    {
        #region IChangeValue 成员

        public ChangeValueInfo<decimal> CheckAndChange(string value)
        {
            decimal changeValue = default(decimal);
            var hasChange = decimal.TryParse(value, out changeValue);

            return new ChangeValueInfo<decimal>()
            {
                HasChange = hasChange,
                ChangeResult = changeValue
            };
        }

        #endregion
    }

    /// <summary>
    /// double类型转换接口定义
    /// </summary>
    internal class DoubleChangeValue : IChangeValue<double>
    {
        #region IChangeValue 成员

        public ChangeValueInfo<double> CheckAndChange(string value)
        {
            double changeValue = default(double);
            var hasChange = double.TryParse(value, out changeValue);

            return new ChangeValueInfo<double>()
            {
                HasChange = hasChange,
                ChangeResult = changeValue
            };
        }

        #endregion
    }

    /// <summary>
    /// float 类型转换接口定义
    /// </summary>
    internal class FloatChangeValue : IChangeValue<float>
    {
        #region IChangeValue 成员

        public ChangeValueInfo<float> CheckAndChange(string value)
        {
            float changeValue = default(float);
            var hasChange = float.TryParse(value, out changeValue);

            return new ChangeValueInfo<float>()
            {
                HasChange = hasChange,
                ChangeResult = changeValue
            };
        }

        #endregion
    }

    /// <summary>
    /// int 类型转换接口定义
    /// </summary>
    internal class IntChangeValue : IChangeValue<int>
    {
        #region IChangeValue 成员

        public ChangeValueInfo<int> CheckAndChange(string value)
        {
            int changeValue = default(int);
            var hasChange = int.TryParse(value, out changeValue);

            return new ChangeValueInfo<int>()
            {
                HasChange = hasChange,
                ChangeResult = changeValue
            };
        }

        #endregion
    }

    /// <summary>
    /// long 类型转换接口定义
    /// </summary>
    internal class LongChangeValue : IChangeValue<long>
    {
        #region IChangeValue 成员

        public ChangeValueInfo<long> CheckAndChange(string value)
        {
            long changeValue = default(long);
            var hasChange = long.TryParse(value, out changeValue);

            return new ChangeValueInfo<long>()
            {
                HasChange = hasChange,
                ChangeResult = changeValue
            };
        }

        #endregion
    }

    /// <summary>
    /// short 类型转换接口定义
    /// </summary>
    internal class ShortChangeValue : IChangeValue<short>
    {
        #region IChangeValue 成员

        public ChangeValueInfo<short> CheckAndChange(string value)
        {
            short changeValue = default(short);
            var hasChange = short.TryParse(value, out changeValue);

            return new ChangeValueInfo<short>()
            {
                HasChange = hasChange,
                ChangeResult = changeValue
            };
        }

        #endregion
    }
}
