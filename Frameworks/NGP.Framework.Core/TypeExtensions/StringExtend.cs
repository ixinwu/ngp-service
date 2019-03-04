/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * StringExtend Description:
 * 字符串扩展
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NGP.Framework.Core
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtend
    {
        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="value">获取的对象</param>
        /// <returns>返回结果</returns>
        public static string EnsureNumericOnly(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : new string(value.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// 确保字符串不超过允许的最大长度
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">如果原始字符串被缩短，则要添加到结尾的字符串</param>
        /// <returns>输入字符串（如果其长度正常）；否则，截断输入字符串</returns>
        public static string EnsureMaximumLength(this string str, int maxLength, string postfix = null)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (str.Length <= maxLength)
                return str;

            var pLen = postfix?.Length ?? 0;

            var result = str.Substring(0, maxLength - pLen);
            if (!string.IsNullOrEmpty(postfix))
            {
                result += postfix;
            }

            return result;
        }

        /// <summary>
        /// 确保字符串不为空
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(this string str)
        {
            return str ?? string.Empty;
        }

        /// <summary>
        /// 字符串分割扩展
        /// </summary>
        /// <param name="input">原始字符串</param>
        /// <param name="separator">分割符号</param>
        /// <returns>分割结果</returns>
        public static IEnumerable<string> SplitAndRemoveEmptyRepeat(this string input,
            params string[] separator)
        {
            IEnumerable<string> result = new List<string>();
            if (string.IsNullOrWhiteSpace(input) || separator.IsNullOrEmpty())
            {
                return result;
            }
            result = input.Split(separator, StringSplitOptions.RemoveEmptyEntries).RemoveEmptyRepeat();
            return result;
        }

        /// <summary>
        /// 格式化字符串列表（去除空白和重复项）
        /// </summary>
        /// <param name="source">源</param>
        /// <returns>格式化结果</returns>
        public static IEnumerable<string> RemoveEmptyRepeat(this IEnumerable<string> source)
        {
            return source.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct();
        }

        /// <summary>
        /// 根据最大长度截取字符串
        /// </summary>
        /// <param name="value">截取源</param>
        /// <param name="maxLength">截取长度</param>
        /// <returns>截取结果</returns>
        public static string EnsureMaximumLength(this string value, int maxLength)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }
            return value;
        }

        /// <summary>
        /// 指示指定的字符串是空字符串还是空字符串
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            return stringsToValidate.Any(string.IsNullOrEmpty);
        }

        #region 加密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Encrypt(string src)
        {
            bool flag = src == "";
            string result;
            if (flag)
            {
                result = src;
            }
            else
            {
                Encoding unicode = Encoding.Unicode;
                byte[] bytes = unicode.GetBytes(src);
                byte[] inArray = Encrypt(bytes);
                string text = Convert.ToBase64String(inArray);
                result = text;
            }
            return result;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static byte[] Encrypt(byte[] input)
        {
            byte[] result;
            using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
            {
                ICryptoTransform cryptoTransform = dESCryptoServiceProvider.CreateEncryptor(_rgbKey, _rgbIV);
                byte[] array = cryptoTransform.TransformFinalBlock(input, 0, input.Length);
                result = array;
            }
            return result;
        }
        #endregion

        #region 解密
        
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Decrypt(string src)
        {
            bool flag = src == "";
            string result;
            if (flag)
            {
                result = src;
            }
            else
            {
                Encoding unicode = Encoding.Unicode;
                byte[] input = Convert.FromBase64String(src);
                byte[] bytes = Decrypt(input);
                string @string = unicode.GetString(bytes);
                result = @string;
            }
            return result;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] input)
        {
            byte[] result;
            using (DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
            {
                ICryptoTransform cryptoTransform = dESCryptoServiceProvider.CreateDecryptor(_rgbKey, _rgbIV);
                byte[] array = cryptoTransform.TransformFinalBlock(input, 0, input.Length);
                result = array;
            }
            return result;
        }
        #endregion
        /// <summary>
        /// rgb key
        /// </summary>
        private static readonly byte[] _rgbKey = new byte[]
            {
                19,
                144,
                17,
                153,
                147,
                19,
                128,
                18
            };
        /// <summary>
        /// rgb iv
        /// </summary>
        private static readonly byte[] _rgbIV = new byte[]
            {
                8,
                1,
                65,
                57,
                1,
                25,
                153,
                49
            };
    }
}
