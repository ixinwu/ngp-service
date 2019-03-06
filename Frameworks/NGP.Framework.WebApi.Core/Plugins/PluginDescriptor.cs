/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * PluginDescriptor Description:
 * 插件描述
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-28   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Newtonsoft.Json;
using NGP.Framework.Core;
using System;
using System.Reflection;

namespace NGP.Framework.WebApi.Core
{
    /// <summary>
    /// 插件描述
    /// </summary>
    public class PluginDescriptor : IComparable<PluginDescriptor>
    {
        #region Ctor
        /// <summary>
        /// ctor
        /// </summary>
        public PluginDescriptor()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="referencedAssembly">Referenced assembly</param>
        public PluginDescriptor(Assembly referencedAssembly) : this()
        {
            this.ReferencedAssembly = referencedAssembly;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取插件的实例
        /// </summary>
        /// <returns>Plugin instance</returns>
        public IPlugin Instance()
        {
            return Instance<IPlugin>();
        }

        /// <summary>
        /// 获取插件的实例
        /// </summary>
        /// <typeparam name="T">Type of the plugin</typeparam>
        /// <returns>Plugin instance</returns>
        public virtual T Instance<T>() where T : class, IPlugin
        {
            object instance = null;
            try
            {
                instance = Singleton<IEngine>.Instance.Resolve(PluginType);
            }
            catch
            {
                //try resolve
            }

            if (instance == null)
            {
                //not resolved
                instance = Singleton<IEngine>.Instance.ResolveUnregistered(PluginType);
            }

            var typedInstance = instance as T;
            if (typedInstance != null)
                typedInstance.PluginDescriptor = this;

            return typedInstance;
        }

        /// <summary>
        /// 将此实例与指定的PluginDescriptor对象进行比较
        /// </summary>
        /// <param name="other">PluginDescriptor与此实例进行比较</param>
        /// <returns>一个整数，指示此实例是否在排序顺序中与指定参数相同，位于或出现在同一位置</returns>
        public int CompareTo(PluginDescriptor other)
        {
            if (DisplayOrder != other.DisplayOrder)
                return DisplayOrder.CompareTo(other.DisplayOrder);

            return FriendlyName.CompareTo(other.FriendlyName);
        }

        /// <summary>
        /// 以字符串形式返回插件
        /// </summary>
        /// <returns>Value of the FriendlyName</returns>
        public override string ToString()
        {
            return FriendlyName;
        }

        /// <summary>
        /// 确定此实例和另一个指定的PluginDescriptor对象是否具有相同的SystemName
        /// </summary>
        /// <param name="value">PluginDescriptor与此实例进行比较</param>
        /// <returns>如果value参数的SystemName与此实例的SystemName相同，则为true;否则为false。 否则，是的</returns>
        public override bool Equals(object value)
        {
            return SystemName?.Equals((value as PluginDescriptor)?.SystemName) ?? false;
        }

        /// <summary>
        /// 返回此插件描述符的哈希码
        /// </summary>
        /// <returns>一个32位有符号整数哈希码</returns>
        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 插件组
        /// </summary>
        [JsonProperty(PropertyName = "Group")]
        public virtual string Group { get; set; }

        /// <summary>
        /// friendly name
        /// </summary>
        [JsonProperty(PropertyName = "FriendlyName")]
        public virtual string FriendlyName { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        [JsonProperty(PropertyName = "SystemName")]
        public virtual string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        [JsonProperty(PropertyName = "DisplayOrder")]
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 程序集文件
        /// </summary>
        [JsonProperty(PropertyName = "FileName")]
        public virtual string AssemblyFileName { get; set; }

        /// <summary>
        /// 是否已经安装
        /// </summary>
        [JsonIgnore]
        public virtual bool Installed { get; set; }

        /// <summary>
        /// 插件类型
        /// </summary>
        [JsonIgnore]
        public virtual Type PluginType { get; set; }

        /// <summary>
        /// 原始程序集
        /// </summary>
        [JsonIgnore]
        public virtual string OriginalAssemblyFile { get; internal set; }

        /// <summary>
        /// 引用程序集
        /// </summary>
        [JsonIgnore]
        public virtual Assembly ReferencedAssembly { get; internal set; }

        #endregion
    }
}