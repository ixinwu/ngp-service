/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * IWebHelper Description:
 * web helper接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System.Net;

namespace NGP.Framework.Core
{
    /// <summary>
    /// web helper接口
    /// </summary>
    public partial interface IWebHelper
    {
        /// <summary>
        /// 获取url来源
        /// </summary>
        /// <returns>URL referrer</returns>
        string GetUrlReferrer();

        /// <summary>
        /// 从HTTP上下文获取IP地址
        /// </summary>
        /// <returns>IP地址字符串</returns>
        string GetCurrentIpAddress();

        /// <summary>
        /// 获取请求url
        /// </summary>
        /// <param name="includeQueryString">指示是否包含查询字符串的值</param>
        /// <param name="useSsl">指示是否获取SSL安全页面URL的值。 传递null以自动确定</param>
        /// <param name="lowercaseUrl">指示是否为小写URL的值</param>
        /// <returns>URL</returns>
        string GetThisRequestUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false);

        /// <summary>
        /// 获取一个值，该值指示当前连接是否受保护
        /// </summary>
        /// <returns>如果它是安全的，则为真，否则为假</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// 获取host
        /// </summary>
        /// <param name="useSsl">是否获取SSL安全URL</param>
        /// <returns>host位置</returns>
        string GetStoreHost(bool useSsl);

        /// <summary>
        /// 获取位置
        /// </summary>
        /// <param name="useSsl">是否获取SSL安全URL; 传递null以自动确定</param>
        /// <returns>location</returns>
        string GetStoreLocation(bool? useSsl = null);

        /// <summary>
        /// 修改URL的查询字符串
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="key">查询要添加的参数键</param>
        /// <param name="values">查询要添加的参数值</param>
        /// <returns>传递查询参数的新URL</returns>
        string ModifyQueryString(string url, string key, params string[] values);

        /// <summary>
        /// 从URL中删除查询参数
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="key">查询要移除的参数键</param>
        /// <param name="value">查询要移除的参数值</param>
        /// <returns>没有传递查询参数的新URL</returns>
        string RemoveQueryString(string url, string key, string value = null);

        /// <summary>
        /// 是否指定了IP地址
        /// </summary>
        /// <param name="address">IP address</param>
        /// <returns>Result</returns>
        bool IsIpAddressSet(IPAddress address);

        /// <summary>
        /// 按名称获取查询字符串值
        /// </summary>
        /// <typeparam name="T">Returned value type</typeparam>
        /// <param name="name">Query parameter name</param>
        /// <returns>Query string value</returns>
        T QueryString<T>(string name);

        /// <summary>
        /// 重启应用程序域
        /// </summary>
        /// <param name="makeRedirect">一个值，指示我们是否应该在重新启动后进行重定向</param>
        void RestartAppDomain(bool makeRedirect = false);

        /// <summary>
        /// 获取一个值，该值指示是否将客户端重定向到新位置
        /// </summary>
        bool IsRequestBeingRedirected { get; }

        /// <summary>
        /// 获取当前的HTTP请求协议
        /// </summary>
        string CurrentRequestProtocol { get; }
    }
}
