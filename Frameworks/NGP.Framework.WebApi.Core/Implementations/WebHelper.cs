/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * WebHelper Description:
 * web helper
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using NGP.Framework.Core;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// web helper
    /// </summary>
    public partial class WebHelper : IWebHelper
    {
        #region Fields 

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INGPFileProvider _fileProvider;

        #endregion

        #region Ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="fileProvider"></param>
        public WebHelper(
            IHttpContextAccessor httpContextAccessor,
            INGPFileProvider fileProvider)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._fileProvider = fileProvider;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 检查当前的HTTP请求是否可用
        /// </summary>
        /// <returns>如果可用则为真; 否则是假的</returns>
        protected virtual bool IsRequestAvailable()
        {
            if (_httpContextAccessor?.HttpContext == null)
                return false;

            try
            {
                if (_httpContextAccessor.HttpContext.Request == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 写入web.config
        /// </summary>
        /// <returns></returns>
        protected virtual bool TryWriteWebConfig()
        {
            try
            {
                _fileProvider.SetLastWriteTimeUtc(_fileProvider.MapPath(NGPInfrastructureDefaults.WebConfigPath), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取url来源
        /// </summary>
        /// <returns>URL referrer</returns>
        public virtual string GetUrlReferrer()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            // 在某些情况下，URL referrer为null（例如，在IE 8中）
            return _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Referer];
        }

        /// <summary>
        /// 从HTTP上下文获取IP地址
        /// </summary>
        /// <returns>IP地址字符串</returns>
        public virtual string GetCurrentIpAddress()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            var result = string.Empty;
            try
            {
                // 首先尝试从转发的标头中获取IP地址
                if (_httpContextAccessor.HttpContext.Request.Headers != null)
                {
                    // X-Forwarded-For（XFF）HTTP头字段是用于识别通过HTTP代理或负载均衡器连接到Web服务器的客户端的原始IP地址的事实标准
                    var forwardedHttpHeaderKey = "X-FORWARDED-FOR";                    

                    var forwardedHeader = _httpContextAccessor.HttpContext.Request.Headers[forwardedHttpHeaderKey];
                    if (!StringValues.IsNullOrEmpty(forwardedHeader))
                        result = forwardedHeader.FirstOrDefault();
                }

                // 如果此标头不存在，请尝试获取连接远程IP地址
                if (string.IsNullOrEmpty(result) && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                    result = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                return string.Empty;
            }

            // 一些验证
            if (result != null && result.Equals(IPAddress.IPv6Loopback.ToString(), StringComparison.InvariantCultureIgnoreCase))
                result = IPAddress.Loopback.ToString();

            // “TryParse”不支持带端口号的IPv4
            if (IPAddress.TryParse(result ?? string.Empty, out var ip))
                // IP地址有效 
                result = ip.ToString();
            else if (!string.IsNullOrEmpty(result))
                // 删除端口
                result = result.Split(':').FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 获取请求url
        /// </summary>
        /// <param name="includeQueryString">指示是否包含查询字符串的值</param>
        /// <param name="useSsl">指示是否获取SSL安全页面URL的值。 传递null以自动确定</param>
        /// <param name="lowercaseUrl">指示是否为小写URL的值</param>
        /// <returns>URL</returns>
        public virtual string GetThisRequestUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            // 获取位置
            var storeLocation = GetStoreLocation(useSsl ?? IsCurrentConnectionSecured());

            // 添加URL的本地路径
            var pageUrl = $"{storeLocation.TrimEnd('/')}{_httpContextAccessor.HttpContext.Request.Path}";

            // 将查询字符串添加到URL
            if (includeQueryString)
                pageUrl = $"{pageUrl}{_httpContextAccessor.HttpContext.Request.QueryString}";

            // 是否将URL转换为小写
            if (lowercaseUrl)
                pageUrl = pageUrl.ToLowerInvariant();

            return pageUrl;
        }

        /// <summary>
        /// 获取一个值，该值指示当前连接是否受保护
        /// </summary>
        /// <returns>如果它是安全的，则为真，否则为假</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            if (!IsRequestAvailable())
                return false;

            return _httpContextAccessor.HttpContext.Request.IsHttps;
        }

        /// <summary>
        /// 获取host
        /// </summary>
        /// <param name="useSsl">是否获取SSL安全URL</param>
        /// <returns>host位置</returns>
        public virtual string GetStoreHost(bool useSsl)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            // 尝试从请求HOST头获取
            var hostHeader = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];
            if (StringValues.IsNullOrEmpty(hostHeader))
                return string.Empty;

            // 将方案添加到URL
            var storeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}{Uri.SchemeDelimiter}{hostHeader.FirstOrDefault()}";

            // 确保以斜杠结束
            storeHost = $"{storeHost.TrimEnd('/')}/";

            return storeHost;
        }

        /// <summary>
        /// 是否指定了IP地址
        /// </summary>
        /// <param name="address">IP address</param>
        /// <returns>Result</returns>
        public virtual bool IsIpAddressSet(IPAddress address)
        {
            return address != null && address.ToString() != IPAddress.IPv6Loopback.ToString();
        }

        /// <summary>
        /// 获取位置
        /// </summary>
        /// <param name="useSsl">是否获取SSL安全URL; 传递null以自动确定</param>
        /// <returns>location</returns>
        public virtual string GetStoreLocation(bool? useSsl = null)
        {
            var storeLocation = string.Empty;

            // 获取host
            var storeHost = GetStoreHost(useSsl ?? IsCurrentConnectionSecured());
            if (!string.IsNullOrEmpty(storeHost))
            {
                // 如果存在，添加应用程序路径库
                storeLocation = IsRequestAvailable() ? $"{storeHost.TrimEnd('/')}{_httpContextAccessor.HttpContext.Request.PathBase}" : storeHost;
            }

            // 确保以斜杠结束
            storeLocation = $"{storeLocation.TrimEnd('/')}/";

            return storeLocation;
        }

        /// <summary>
        /// 修改URL的查询字符串
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="key">查询要添加的参数键</param>
        /// <param name="values">查询要添加的参数值</param>
        /// <returns>传递查询参数的新URL</returns>
        public virtual string ModifyQueryString(string url, string key, params string[] values)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            if (string.IsNullOrEmpty(key))
                return url;

            // 获取当前参数
            var uri = new Uri(url);
            var queryParameters = QueryHelpers.ParseQuery(uri.Query);

            // 添加参数
            queryParameters[key] = new StringValues(values);
            var queryBuilder = new QueryBuilder(queryParameters
                .ToDictionary(parameter => parameter.Key, parameter => parameter.Value.ToString()));

            // 使用传递的查询参数创建新URL
            url = $"{uri.GetLeftPart(UriPartial.Path)}{queryBuilder.ToQueryString()}{uri.Fragment}";

            return url;
        }

        /// <summary>
        /// 从URL中删除查询参数
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="key">查询要移除的参数键</param>
        /// <param name="value">查询要移除的参数值</param>
        /// <returns>没有传递查询参数的新URL</returns>
        public virtual string RemoveQueryString(string url, string key, string value = null)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            if (string.IsNullOrEmpty(key))
                return url;

            // 获取当前参数
            var uri = new Uri(url);
            var queryParameters = QueryHelpers.ParseQuery(uri.Query)
                .SelectMany(parameter => parameter.Value, (parameter, queryValue) => new KeyValuePair<string, string>(parameter.Key, queryValue))
                .ToList();

            if (!string.IsNullOrEmpty(value))
            {
                // 如果已传递，则删除特定的查询参数值
                queryParameters.RemoveAll(parameter => parameter.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)
                    && parameter.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                // 或通过键删除查询参数
                queryParameters.RemoveAll(parameter => parameter.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            }

            // 创建新的URL而不传递查询参数
            url = $"{uri.GetLeftPart(UriPartial.Path)}{new QueryBuilder(queryParameters).ToQueryString()}{uri.Fragment}";

            return url;
        }

        /// <summary>
        /// 按名称获取查询字符串值
        /// </summary>
        /// <typeparam name="T">Returned value type</typeparam>
        /// <param name="name">Query parameter name</param>
        /// <returns>Query string value</returns>
        public virtual T QueryString<T>(string name)
        {
            if (!IsRequestAvailable())
                return default(T);

            if (StringValues.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Query[name]))
                return default(T);

            return _httpContextAccessor.HttpContext.Request.Query[name].ToString().To<T>();
        }

        /// <summary>
        /// 重启应用程序域
        /// </summary>
        /// <param name="makeRedirect">一个值，指示我们是否应该在重新启动后进行重定向</param>
        public virtual void RestartAppDomain(bool makeRedirect = false)
        {
            //the site will be restarted during the next request automatically
            //_applicationLifetime.StopApplication();

            //"touch" web.config to force restart
            var success = TryWriteWebConfig();
            if (!success)
            {
                throw new NGPException("nopCommerce needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
                    "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
                    "- run the application in a full trust environment, or" + Environment.NewLine +
                    "- give the application write access to the 'web.config' file.");
            }
        }

        /// <summary>
        /// 获取一个值，该值指示是否将客户端重定向到新位置
        /// </summary>
        public virtual bool IsRequestBeingRedirected
        {
            get
            {
                var response = _httpContextAccessor.HttpContext.Response;
                //ASP.NET 4 style - return response.IsRequestBeingRedirected;
                int[] redirectionStatusCodes = { StatusCodes.Status301MovedPermanently, StatusCodes.Status302Found };
                return redirectionStatusCodes.Contains(response.StatusCode);
            }
        }

        /// <summary>
        /// 获取当前的HTTP请求协议
        /// </summary>
        public virtual string CurrentRequestProtocol => IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;

        #endregion
    }
}