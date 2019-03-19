/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * FieldType Description:
 * 字段类型
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-1-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

namespace NGP.Framework.Core
{
    /// <summary>
    /// 字段类型
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// 表单类型
        /// </summary>
        FormType = 0,

        /// <summary>
        /// 关联类型
        /// </summary>
        RelationType,

        /// <summary>
        /// 组类型
        /// </summary>
        GroupType,

        /// <summary>
        /// 人员类型
        /// </summary>
        EmployeeType,

        /// <summary>
        /// 部门类型
        /// </summary>
        DeptType,
    }
}
