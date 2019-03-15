/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * App_Config_DataSet Description:
 * 应用数据集配置
 *
 * Comment 					        Revision	        Date                 Author
 * -----------------------------    --------         --------            -----------
 * Created							1.0		    2019/3/7 14:49:49   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 应用数据集配置
    /// </summary>
    public class App_Config_DataSet : BaseDBEntity
    {
        /// <summary>
        /// 应用key
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 数据集key
        /// </summary>
        public string DataSetKey { get; set; }
        /// <summary>
        /// 数据集名称
        /// </summary>
        public string DataSetName { get; set; }
        /// <summary>
        /// 主表单key
        /// </summary>
        public string MainFormKey { get; set; }
        /// <summary>
        /// 关联id列表
        /// </summary>
        public string RelationIds { get; set; }
    }
}