/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * ILinqParserCommand Description:
 * linq解析命令接口
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/


using System.Collections.Generic;

namespace NGP.Framework.Core
{
    /// <summary>
    /// linq解析命令接口
    /// </summary>
    public interface ILinqParserCommand
    {
        /// <summary>
        /// new guid id methods
        /// </summary>
        string NewId { get; }

        /// <summary>
        /// param key
        /// </summary>
        string ParamKey { get; }

        /// <summary>
        /// select query
        /// </summary>
        /// <param name="distinct"></param>
        /// <param name="selectCommand"></param>
        /// <param name="fromCommand"></param>
        /// <param name="joinCommand"></param>
        /// <param name="whereCommand"></param>
        /// <param name="orderCommand"></param>
        /// <param name="groupCommand"></param>
        /// <returns>query string</returns>
        string SelectQuery(string distinct,
                                   string selectCommand,
                                   string fromCommand,
                                   string joinCommand,
                                   string whereCommand,
                                   string orderCommand,
                                   string groupCommand);

        /// <summary>
        /// select single query
        /// </summary>
        /// <param name="distinct"></param>
        /// <param name="selectCommand"></param>
        /// <param name="fromCommand"></param>
        /// <param name="joinCommand"></param>
        /// <param name="whereCommand"></param>
        /// <param name="orderCommand"></param>
        /// <param name="groupCommand"></param>
        /// <returns>query string</returns>
        string SelectSingleQuery(string distinct,
                                   string selectCommand,
                                   string fromCommand,
                                   string joinCommand,
                                   string whereCommand,
                                   string orderCommand,
                                   string groupCommand);

        /// <summary>
        /// select page query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectCommand"></param>
        /// <param name="fromCommand"></param>
        /// <param name="joinCommand"></param>
        /// <param name="whereCommand"></param>
        /// <param name="groupCommand"></param>
        /// <param name="orderCommand"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageEnd"></param>
        /// <returns>query string</returns>
        string SelectPageQuery<T>(string selectCommand,
                                        string fromCommand,
                                        string joinCommand,
                                        string whereCommand,
                                        string groupCommand,
                                        string orderCommand,
                                        T pageStart,
                                        T pageEnd);

        /// <summary>
        /// select count query
        /// </summary>
        /// <param name="fromCommand"></param>
        /// <param name="joinCommand"></param>
        /// <param name="whereCommand"></param>
        /// <returns></returns>
        string SelectTotalCountQuery(string fromCommand,
            string joinCommand,
            string whereCommand);

        /// <summary>
        /// from join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string FromJoin(List<string> list);

        /// <summary>
        /// param join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string ParamJoin(List<string> list);

        /// <summary>
        /// order join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string OrderJoin(List<string> list);

        /// <summary>
        /// join condition join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string JoinConditionJoin(List<string> list);

        /// <summary>
        /// field join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string FieldJoin(List<string> list);

        /// <summary>
        /// join join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string JoinJoin(List<string> list);

        /// <summary>
        /// select join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string SelectJoin(List<string> list);

        /// <summary>
        /// where command
        /// </summary>
        /// <param name="whereText"></param>
        /// <returns></returns>
        string WhereCommand(string whereText);

        /// <summary>
        /// distinct command
        /// </summary>
        string DistinctCommand { get; }

        /// <summary>
        /// order command
        /// </summary>
        /// <param name="orderText"></param>
        /// <returns></returns>
        string OrderCommand(string orderText);

        /// <summary>
        /// group command
        /// </summary>
        /// <param name="groupText"></param>
        /// <returns></returns>
        string GroupCommand(string groupText);

        /// <summary>
        /// equal command
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        string EqualCommand(string left, string right);

        /// <summary>
        /// and
        /// </summary>
        string AndCommand { get; }

        /// <summary>
        ///  join command
        /// </summary>
        /// <param name="joinDirection"></param>
        /// <param name="joinSchema"></param>
        /// <param name="rename"></param>
        /// <param name="joinCondition"></param>
        /// <returns></returns>
        string JoinCommand(string joinDirection, string joinSchema, string rename, string joinCondition);

        /// <summary>
        /// rename command
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        string RenameCommand(string oldName, string newName);

        /// <summary>
        /// bracket command
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string BracketCommand(string content);

        /// <summary>
        /// not command
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string NotCommand(string content);

        /// <summary>
        /// in command
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        string InCommand(string left, string right);

        /// <summary>
        /// like command
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        string LikeCommand(string left, string right);

        /// <summary>
        /// null equal
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string NullEqualCommand(string expression);

        /// <summary>
        /// null not equal
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string NullNotEqualCommand(string expression);

        /// <summary>
        /// where operator command
        /// </summary>
        /// <param name="left"></param>
        /// <param name="oper"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        string WhereOperatorCommand(string left, string oper, string right);

        /// <summary>
        /// where select compare command
        /// </summary>
        /// <param name="field"></param>
        /// <param name="oper"></param>
        /// <param name="selectCommand"></param>
        /// <returns></returns>
        string WhereSelectCompareCommand(string field, string oper, string selectCommand);

        /// <summary>
        /// where select in command
        /// </summary>
        /// <param name="field"></param>
        /// <param name="selectCommand"></param>
        /// <returns></returns>
        string WhereSelectInCommand(string field, string selectCommand);

        /// <summary>
        /// or command
        /// </summary>
        string OrCommand { get; }

        /// <summary>
        /// order element command
        /// </summary>
        /// <param name="field"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        string OrderElementCommand(string field, string direction);

        /// <summary>
        /// date command
        /// </summary>
        string DateCommand { get; }

        /// <summary>
        /// sum command key
        /// </summary>
        string SumCommandKey { get; }

        /// <summary>
        ///  sum command
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        string SumCommand(string field);

        /// <summary>
        /// count command key
        /// </summary>
        string CountCommandKey { get; }

        /// <summary>
        /// count command
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        string CountCommand(string field);

        /// <summary>
        /// avg command
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        string AvgCommand(string field);

        /// <summary>
        /// avg command key
        /// </summary>
        string AvgCommandKey { get; }

        /// <summary>
        /// like value command
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string LikeValueCommand(string value);

        /// <summary>
        /// date validate command
        /// </summary>
        /// <param name="type"></param>
        /// <param name="compareDate"></param>
        /// <param name="paramDate"></param>
        /// <returns></returns>
        string DateValidateCommand(string type, string compareDate, string paramDate);

        /// <summary>
        /// date part command
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        string DatePartCommand(string type, string dateValue);

        /// <summary>
        /// date add command
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="addNumber"></param>
        /// <returns></returns>
        string DateAddCommand(string type, string target, string addNumber);

        /// <summary>
        ///  date diff command
        /// </summary>
        /// <param name="type"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        string DateDiffCommand(string type, string startDate, string endDate);

        /// <summary>
        /// 判断多数据通过分割(分割列)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="paramKey"></param>
        /// <returns></returns>
        string CheckMulitDataBySpitCommand(string field, string paramKey);

        /// <summary>
        /// 判断多数据通过分割(单列)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="paramKey"></param>
        /// <returns></returns>
        string CheckSingleDataBySpitCommand(string field, string paramKey);

        /// <summary>
        /// 判断多数据通过分割(单列)格式化值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string CheckSingleValueCommand(string value);
    }
}
