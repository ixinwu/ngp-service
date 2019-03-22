/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * LinqParserSqlServerCommand Description:
 * linq解析sqlserver命令
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
    /// linq解析sqlserver命令
    /// </summary>
    public class LinqParserSqlServerCommand : ILinqParserCommand
    {
        /// <summary>
        /// 参数索引
        /// </summary>
        private int index;

        /// <summary>
        /// new guid id methods
        /// </summary>
        /// <returns>guid command</returns>
        public string NewId { get => "NEWID()"; }

        /// <summary>
        /// get param key
        /// </summary>
        /// <returns>param key</returns>
        public string ParamKey { get => string.Format("@{0}", index++); }

        /// <summary>
        /// param command
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ParamCommand(string key) => string.Format("@{0}", key);

        /// <summary>
        /// linq set命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public string LinqSetCommand<T>(string left, T right) => string.Format("{0} : {1}", left, right);

        /// <summary>
        /// linq string formatter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string LinqStringFormatter(string value) => string.Format("\"{0}\"", value);

        /// <summary>
        /// select query
        /// </summary>
        /// <param name="distinct"></param>
        /// <param name="topCommand"></param>
        /// <param name="selectCommand"></param>
        /// <param name="formCommand"></param>
        /// <param name="joinCommand"></param>
        /// <param name="whereCommand"></param>
        /// <param name="orderCommand"></param>
        /// <param name="groupCommand"></param>
        /// <returns>query string</returns>
        public string SelectQuery(string distinct,
            string topCommand,
            string selectCommand,
            string formCommand,
            string joinCommand,
            string whereCommand,
            string orderCommand,
            string groupCommand)
        => string.Format("SELECT {0} {7} {1} \r\n FROM {2} \r\n {3} \r\n {4} \r\n {5} \r\n {6}",
               distinct,
               selectCommand,
               formCommand,
               joinCommand,
               whereCommand,
               orderCommand,
               groupCommand,
               topCommand);

        /// <summary>
        /// select page query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectCommand"></param>
        /// <param name="formCommand"></param>
        /// <param name="joinCommand"></param>
        /// <param name="whereCommand"></param>
        /// <param name="groupCommand"></param>
        /// <param name="orderCommand"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageEnd"></param>
        /// <returns>query string</returns>
        public string SelectPageQuery<T>(string selectCommand,
            string formCommand,
            string joinCommand,
            string whereCommand,
            string groupCommand,
            string orderCommand,
            T pageStart,
            T pageEnd)
        => string.Format("SELECT * FROM \r\n (SELECT ROW_NUMBER() OVER ({5}) RowNumber,{0} \r\n" +
                        " FROM {1} \r\n {2} \r\n {3} \r\n {4}) temp \r\n" +
                        " WHERE temp.RowNumber > {6} and temp.RowNumber <= {7} \r\n",
                selectCommand,
                formCommand,
                joinCommand,
                whereCommand,
                groupCommand,
                orderCommand,
                pageStart,
                pageEnd);

        /// <summary>
        /// select count query
        /// </summary>
        /// <param name="formCommand"></param>
        /// <param name="joinCommand"></param>
        /// <param name="whereCommand"></param>
        /// <returns></returns>
        public string SelectTotalCountQuery(string formCommand,
            string joinCommand,
            string whereCommand)
        => string.Format("SELECT COUNT(1) \r\n FROM {0} \r\n {1} \r\n {2}",
            formCommand,
            joinCommand,
            whereCommand);

        /// <summary>
        /// 插入command
        /// </summary>
        /// <param name="formCommand"></param>
        /// <param name="insertCommand"></param>
        /// <param name="parameterCommand"></param>
        /// <returns></returns>
        public string InsertCommand(string formCommand, string insertCommand, string parameterCommand)
        => string.Format("INSERT INTO {0} \r\n ({1}) \r\n {2}",
            formCommand,
            insertCommand,
            parameterCommand);

        /// <summary>
        /// 更新command
        /// </summary>
        /// <param name="formCommand"></param>
        /// <param name="setCommand"></param>
        /// <param name="whereCommand"></param>
        /// <returns></returns>
        public string UpdateCommand(string formCommand, string setCommand, string whereCommand)
        => string.Format("UPDATE {0} \r\n SET {1} \r\n {2} \r\n",
            formCommand,
            setCommand,
            whereCommand);

        /// <summary>
        /// form join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string JoinForm(IEnumerable<string> list) => string.Join(",", list);

        /// <summary>
        /// param join
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public string JoinParam<T>(IEnumerable<T> list) => string.Join(",", list);

        /// <summary>
        /// order join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string JoinOrder(IEnumerable<string> list) => string.Join(",", list);

        /// <summary>
        /// join condition join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string JoinCondition(IEnumerable<string> list) => string.Join(AndCommand, list);

        /// <summary>
        /// field join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string JoinField(IEnumerable<string> list) => string.Join(",", list);

        /// <summary>
        /// join join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string JoinLine(IEnumerable<string> list) => string.Join(" \r\n", list);

        /// <summary>
        /// join insert
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string JoinInsert(IEnumerable<string> list) => string.Join(" ; \r\n", list);

        /// <summary>
        /// join update
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string JoinUpdate(IEnumerable<string> list) => string.Join(" ; \r\n", list);

        /// <summary>
        /// select join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string JoinSelect(IEnumerable<string> list) => string.Join(",", list);

        /// <summary>
        /// field join
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string JoinSet(IEnumerable<string> list) => string.Join(",", list);

        /// <summary>
        /// where command
        /// </summary>
        /// <param name="whereText"></param>
        /// <returns></returns>
        public string WhereCommand(string whereText) => string.Format("WHERE {0}", whereText);

        /// <summary>
        /// values command
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string ValuesCommand(string values) => string.Format("VALUES ({0})", values);

        /// <summary>
        /// distinct command
        /// </summary>
        /// <returns></returns>
        public string DistinctCommand { get => "DISTINCT"; }

        /// <summary>
        /// order command
        /// </summary>
        /// <param name="orderText"></param>
        /// <returns></returns>
        public string OrderCommand(string orderText) => string.Format(" ORDER BY {0} ", orderText);

        /// <summary>
        /// group command
        /// </summary>
        /// <param name="groupText"></param>
        /// <returns></returns>
        public string GroupCommand(string groupText) => string.Format(" GROUP BY {0} ", groupText);

        /// <summary>
        /// equal command
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public string EqualCommand<TValue>(string left, TValue right) => string.Format("{0} = {1}", left, right);

        /// <summary>
        /// and
        /// </summary>
        /// <returns></returns>
        public string AndCommand { get => " AND "; }

        /// <summary>
        ///  join command
        /// </summary>
        /// <param name="joinDirection"></param>
        /// <param name="joinSchema"></param>
        /// <param name="rename"></param>
        /// <param name="joinCondition"></param>
        /// <returns></returns>
        public string JoinCommand(string joinDirection, string joinSchema, string rename, string joinCondition)
             => string.Format("{0} JOIN {1} AS {2} ON {3}",
                 joinDirection,
                 joinSchema,
                 rename,
                 joinCondition);

        /// <summary>
        ///  join command
        /// </summary>
        /// <param name="joinSchema"></param>
        /// <param name="joinCondition"></param>
        /// <returns></returns>
        public string LeftJoinCommand(string joinSchema, string joinCondition)
             => string.Format("LEFT JOIN {0} ON {1}",
                 joinSchema,
                 joinCondition);

        /// <summary>
        /// rename command
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public string RenameCommand(string oldName, string newName)
            => string.Format(" {0} AS {1} ", oldName, newName);

        /// <summary>
        /// bracket command
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string BracketCommand(string content) => string.Format("({0})", content);

        /// <summary>
        /// not command
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string NotCommand(string content) => string.Format("NOT ({0})", content);

        /// <summary>
        /// in command
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public string InCommand(string left, string right) => string.Format("{0} IN ({1})", left, right);

        /// <summary>
        /// like command
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public string LikeCommand(string left, string right) => string.Format("{0} LIKE {1}", left, right);

        /// <summary>
        /// null equal
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string NullEqualCommand(string expression) => string.Format("{0} IS NULL", expression);

        /// <summary>
        /// null not equal
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string NullNotEqualCommand(string expression) => string.Format("{0} IS NOT NULL", expression);

        /// <summary>
        /// where operator command
        /// </summary>
        /// <param name="left"></param>
        /// <param name="op"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public string WhereOperatorCommand(string left, string op, string right)
            => string.Format("{0} {1} {2}", left, op, right);

        /// <summary>
        /// where select compare command
        /// </summary>
        /// <param name="field"></param>
        /// <param name="op"></param>
        /// <param name="selectCommand"></param>
        /// <returns></returns>
        public string WhereSelectCompareCommand(string field, string op, string selectCommand)
            => string.Format("{0} {1} ({2})", field, op, selectCommand);

        /// <summary>
        /// where select in command
        /// </summary>
        /// <param name="field"></param>
        /// <param name="selectCommand"></param>
        /// <returns></returns>
        public string WhereSelectInCommand(string field, string selectCommand)
            => string.Format("{0} IN ({1})", field, selectCommand);

        /// <summary>
        /// or command
        /// </summary>
        public string OrCommand { get => " OR "; }

        /// <summary>
        /// order element command
        /// </summary>
        /// <param name="field"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public string OrderElementCommand(string field, string direction) => string.Format("{0} {1} ", field, direction);

        /// <summary>
        /// date command
        /// </summary>
        public string DateCommand { get => "GETDATE()"; }

        /// <summary>
        ///  sum command
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string SumCommand(string field) => string.Format("SUM({0})", field);

        /// <summary>
        /// sum command key
        /// </summary>
        public string SumCommandKey { get => "SUM"; }

        /// <summary>
        /// count command
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string CountCommand(string field) => string.Format("COUNT({0})", field);

        /// <summary>
        /// count command key
        /// </summary>
        public string CountCommandKey { get => "COUNT"; }

        /// <summary>
        /// avg command
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string AvgCommand(string field) => string.Format("AVG({0})", field);

        /// <summary>
        /// avg command key
        /// </summary>
        public string AvgCommandKey { get => "AVG"; }

        /// <summary>
        /// null command key
        /// </summary>
        public string NullCommandKey { get => "NULL"; }

        /// <summary>
        /// top command
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string TopCommand(int number) => string.Format("TOP {0}", number);

        /// <summary>
        /// like value command
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string LikeValueCommand(string value) => string.Format("%{0}%", value);

        /// <summary>
        /// date validate command
        /// </summary>
        /// <param name="type"></param>
        /// <param name="compareDate"></param>
        /// <param name="paramDate"></param>
        /// <returns></returns>
        public string DateValidateCommand(string type, string compareDate, string paramDate)
            => string.Format("({1}  BETWEEN DATEADD({0}, DATEDIFF({0},0, {2}), 0)  \n" +
                        "AND DATEADD(ms,-3, DATEADD({0}, DATEDIFF({0},0, {2})+1, 0)))",
                type,
                compareDate,
                paramDate);

        /// <summary>
        /// date part command
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        public string DatePartCommand(string type, string dateValue)
            => string.Format("DATEPART({0},{1})", type, dateValue);

        /// <summary>
        /// date add command
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="addNumber"></param>
        /// <returns></returns>
        public string DateAddCommand(string type, string target, string addNumber)
            => string.Format("DATEADD({0},{1},{2})", type, target, addNumber);

        /// <summary>
        ///  date diff command
        /// </summary>
        /// <param name="type"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public string DateDiffCommand(string type, string startDate, string endDate)
            => string.Format("DATEDIFF({0},{1},{2})", type, startDate, endDate);

        /// <summary>
        /// 判断多数据通过分割(分割列)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="paramKey"></param>
        /// <returns></returns>
        public string CheckMulitDataBySpitCommand(string field, string paramKey)
             => string.Format("(SELECT COUNT(1) \n" +
                 "FROM (\n" +
                 "SELECT B.CheckTempColumn \n" +
                 "FROM \n" +
                 "(SELECT CheckTempColumn=" +
                 "CONVERT(XML,' <root> <v>'+REPLACE(CheckTempColumn,',',' </v> <v>')+' </v> </root>') \n" +
                 "FROM \n" +
                 "(SELECT {0} CheckTempColumn) G) A \n" +
                 "OUTER APPLY \n" +
                 "(SELECT CheckTempColumn=C.V.value('.','nvarchar(100)') \n" +
                 " FROM A.CheckTempColumn.nodes('/root/v')C(V))B ) TEMP1 \n" +
                 "INNER JOIN \n" +
                 "(SELECT B.CheckTempColumn \n" +
                 " FROM \n" +
                 "(SELECT CheckTempColumn=" +
                 "CONVERT(XML,' <root> <v>'+REPLACE(CheckTempColumn,',',' </v> <v>')+' </v> </root>') \n" +
                 " FROM \n" +
                 " (SELECT ({1})CheckTempColumn) G) A \n" +
                 "OUTER APPLY \n" +
                 "(SELECT CheckTempColumn=C.V.value('.','nvarchar(100)') \n" +
                 "FROM A.CheckTempColumn.nodes('/root/v')C(V))B ) TEMP2 \n" +
                 "ON TEMP1.CheckTempColumn = TEMP2.CheckTempColumn)>0 ", field, paramKey);

        /// <summary>
        /// 判断多数据通过分割(单列)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="paramKey"></param>
        /// <returns></returns>
        public string CheckSingleDataBySpitCommand(string field, string paramKey)
            => string.Format("','+{0}+',' LIKE {1}", field, paramKey);

        /// <summary>
        /// 判断多数据通过分割(单列)格式化值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string CheckSingleValueCommand(string value) => string.Format("%,{0},%", value);
    }
}
