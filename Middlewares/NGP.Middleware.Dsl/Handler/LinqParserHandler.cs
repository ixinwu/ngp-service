/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * LinqParserHandler Description:
 * linq解析处理器
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019-3-15   hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using NGP.Framework.Core;
using NGP.Middleware.Dsl.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGP.Middleware.Dsl.Handler
{
    /// <summary>
    /// linq解析处理器
    /// </summary>
    public class LinqParserHandler : LinqParserBaseListener, ILinqParserHandler
    {
        #region private fields
        /// <summary>
        /// 解析command
        /// </summary>
        private readonly ILinqParserCommand _parserCommand;

        /// <summary>
        /// sql 参数字段
        /// </summary>
        private readonly IDictionary<string, object> _params = new Dictionary<string, object>();

        /// <summary>
        /// 生成对象列表
        /// </summary>
        private readonly List<DynamicGenerateObject> _generateObjects = new List<DynamicGenerateObject>();

        /// <summary>
        /// 参数树
        /// </summary>
        private readonly ParseTreeProperty<string> _statementTree = new ParseTreeProperty<string>();

        /// <summary>
        /// 请求对象
        /// </summary>
        private LinqParserRequest _parserRequest;

        /// <summary>
        /// 解析类型
        /// </summary>
        private LinqParserType _linqParserType;
        #endregion

        #region ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="parserCommand"></param>
        public LinqParserHandler(ILinqParserCommand parserCommand)
        {
            _parserCommand = parserCommand;
        }
        #endregion

        #region Unitils
        /// <summary>
        /// 设定参数
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="command"></param>
        private void SetStatement(IParseTree ctx, string command) => _statementTree.Put(ctx, command);

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private string GetStatement(IParseTree ctx) => _statementTree.Get(ctx);

        /// <summary>
        /// 通过值获取参数key
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetParamCommandByValue(object value)
        {
            var key = _params.FirstOrDefault(s => s.Value.Equals(value)).Key;
            if (!string.IsNullOrWhiteSpace(key))
            {
                return key;
            }

            key = _parserCommand.ParamKey;
            _params[key] = value;
            return key;
        }
        #endregion

        #region ILinqParserHandler Methods
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>解析结果</returns>
        public LinqParserResponse Resolve(LinqParserRequest request)
        {
            _parserRequest = request;

            //执行解析
            var stream = new AntlrInputStream(request.DslContent);
            var lexer = new LinqParserLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new LinqParserParser(tokens);
            parser.BuildParseTree = true;
            IParseTree tree = parser.linqStatement();

            ParseTreeWalker walker = new ParseTreeWalker();
            walker.Walk(this, tree);

            // 获取解析command
            var commandText = GetStatement(tree);

            // 生成返回结果
            var command = new ExcuteSqlCommand(commandText);
            foreach (var param in _params)
            {
                command.ParameterCollection[param.Key] = param.Value;
            }
            return new LinqParserResponse
            {
                Command = command,
                GenerateObjects = _generateObjects,
                ParserType = _linqParserType
            };
        }
        #endregion

        #region LinqParserBaseListener Methods
        #region linq statement
        /// <summary>
        /// linq statement
        /// </summary>
        /// <param name="context"></param>
        public override void ExitLinqStatement([NotNull] LinqParserParser.LinqStatementContext context)
        {
            if (context.orderStatement() != null)
            {
                SetStatement(context, GetStatement(context.orderStatement()));
                return;
            }

            if (context.selectStatement() != null)
            {
                _linqParserType = LinqParserType.Query;
                SetStatement(context, GetStatement(context.selectStatement()));
                return;
            }

            _linqParserType = LinqParserType.ExecuteNonQuery;
            if (context.updateStatements() != null)
            {
                SetStatement(context, GetStatement(context.updateStatements()));
                return;
            }
            if (context.insertStatements() != null)
            {
                SetStatement(context, GetStatement(context.insertStatements()));
                return;
            }
            SetStatement(context, GetStatement(context.whereStatement()));
        }
        #endregion

        #region select,insert,update,join,form,order,group statemelt define
        /// <summary>
        /// select statement
        /// </summary>
        /// <param name="context"></param>
        public override void ExitSelectStatement([NotNull] LinqParserParser.SelectStatementContext context)
        {
            var formList = new List<string>();
            foreach (var form in context.formStatement())
            {
                formList.Add(GetStatement(form));
            }
            var formCommand = _parserCommand.JoinForm(formList);

            var joinCommand = "";
            if (context.joinStatement() != null)
            {
                var joinList = new List<string>();
                foreach (var join in context.joinStatement())
                {
                    joinList.Add(GetStatement(join));
                }
                joinCommand = _parserCommand.JoinLine(joinList);
            }

            var whereCommand = "";
            if (context.whereStatement() != null)
            {
                whereCommand = _parserCommand.WhereCommand(GetStatement(context.whereStatement()));
            }

            var groupCommand = "";
            if (context.groupStatement() != null)
            {
                groupCommand = GetStatement(context.groupStatement());
            }
            var selectCommand = GetStatement(context.selectElements());

            var orderCommand = "";
            if (context.orderStatement() != null)
            {
                orderCommand = GetStatement(context.orderStatement());
            }

            var distinct = "";
            if (context.DISTINCT() != null)
            {
                distinct = _parserCommand.DistinctCommand;
            }
            var commandText = "";
            var topCommand = string.Empty;
            if (context.TOP() != null)
            {
                topCommand = _parserCommand.TopCommand(context.INT().LastOrDefault().GetText().To(1));
            }

            if (context.LIMIT() == null)
            {
                commandText = _parserCommand.SelectQuery(distinct,
                    topCommand,
                    selectCommand,
                    formCommand,
                    joinCommand,
                    whereCommand,
                    orderCommand,
                    groupCommand);
                SetStatement(context, commandText);
                return;
            }

            var startIndex = context.INT(0).GetText().To<int>();
            var startParamKey = GetParamCommandByValue(startIndex);
            var endIndex = context.INT(1).GetText().To<int>() + startIndex;
            var endParamKey = GetParamCommandByValue(endIndex);
            commandText = _parserCommand.SelectPageQuery(selectCommand,
                    formCommand,
                    joinCommand,
                    whereCommand,
                    groupCommand,
                    orderCommand,
                    startParamKey,
                    endParamKey);
            SetStatement(context, commandText);
        }

        /// <summary>
        /// form statement
        /// </summary>
        /// <param name="context"></param>
        public override void ExitFormStatement([NotNull] LinqParserParser.FormStatementContext context)
        {
            SetStatement(context, _parserCommand.RenameCommand(
                GetStatement(context.schemaStatement()),
                GetStatement(context.asParam())));
        }

        /// <summary>
        /// join statement
        /// </summary>
        /// <param name="context"></param>
        public override void ExitJoinStatement([NotNull] LinqParserParser.JoinStatementContext context)
        {
            var joinDirection = "";
            if (context.JOINDIRECTION() != null)
            {
                joinDirection = context.JOINDIRECTION().GetText().ToUpper();
            }
            var schema = GetStatement(context.schemaStatement());
            var left = GetStatement(context.joinLeftElements());
            var right = GetStatement(context.joinRightElements());
            var leftFields = left.Split(",");
            var rightFields = right.Split(",");
            var joinConditionList = new List<string>();
            for (int index = 0; index < leftFields.Length; index++)
            {
                joinConditionList.Add(_parserCommand.EqualCommand(leftFields[index],
                        rightFields[index]));
            }
            var joinCondition = _parserCommand.JoinCondition(joinConditionList);
            var joinCommand = _parserCommand.JoinCommand(joinDirection,
                    schema,
                    GetStatement(context.asParam()),
                    joinCondition);
            SetStatement(context, joinCommand);
        }

        /// <summary>
        /// schema statement
        /// </summary>
        /// <param name="context"></param>
        public override void ExitSchemaStatement([NotNull] LinqParserParser.SchemaStatementContext context)
        {
            var schema = "";
            if (context.formParam() != null)
            {
                schema = GetStatement(context.formParam());
            }
            else
            {
                schema = _parserCommand.BracketCommand(GetStatement(context.selectStatement()));
            }
            SetStatement(context, schema);
        }

        /// <summary>
        /// group statement
        /// </summary>
        /// <param name="context"></param>
        public override void ExitGroupStatement([NotNull] LinqParserParser.GroupStatementContext context)
        {
            SetStatement(context, _parserCommand.GroupCommand(GetStatement(context.fieldElements())));
        }

        /// <summary>
        /// order statement
        /// </summary>
        /// <param name="context"></param>
        public override void ExitOrderStatement([NotNull] LinqParserParser.OrderStatementContext context)
        {
            var orderList = new List<string>();
            foreach (var field in context.orderElement())
            {
                orderList.Add(GetStatement(field));
            }
            var orderFields = _parserCommand.JoinOrder(orderList);
            SetStatement(context, _parserCommand.OrderCommand(orderFields));
        }

        /// <summary>
        /// update statements
        /// </summary>
        /// <param name="context"></param>
        public override void ExitUpdateStatements([NotNull] LinqParserParser.UpdateStatementsContext context)
        {
            var statementList = new List<string>();
            foreach (var child in context.updateStatement())
            {
                statementList.Add(GetStatement(child));
            }
            SetStatement(context, _parserCommand.JoinUpdate(statementList));
        }

        /// <summary>
        /// update statement
        /// </summary>
        /// <param name="context"></param>
        public override void ExitUpdateStatement([NotNull] LinqParserParser.UpdateStatementContext context)
        {
            var formCommand = GetStatement(context.formParam());
            var setCommand = GetStatement(context.updateSetElements());
            var whereCommand = string.Empty;
            if (context.WHERE() != null)
            {
                whereCommand = _parserCommand.WhereCommand(GetStatement(context.whereStatement()));
            }
            SetStatement(context, _parserCommand.UpdateCommand(formCommand, setCommand, whereCommand));
        }

        /// <summary>
        /// insert statement
        /// </summary>
        /// <param name="context"></param>
        public override void ExitInsertStatements([NotNull] LinqParserParser.InsertStatementsContext context)
        {
            var statementList = new List<string>();
            foreach (var child in context.insertStatement())
            {
                statementList.Add(GetStatement(child));
            }
            SetStatement(context, _parserCommand.JoinUpdate(statementList));
        }

        /// <summary>
        /// insert values
        /// </summary>
        /// <param name="context"></param>
        public override void ExitInsertValues([NotNull] LinqParserParser.InsertValuesContext context)
        {
            var formCommand = GetStatement(context.formParam());
            var fieldCommand = GetStatement(context.fieldElements());
            var valueCommand = _parserCommand.ValuesCommand(GetStatement(context.insertParamElements()));
            SetStatement(context, _parserCommand.InsertParamCommand(formCommand, fieldCommand, valueCommand));
        }

        /// <summary>
        /// insert select
        /// </summary>
        /// <param name="context"></param>
        public override void ExitInsertSelect([NotNull] LinqParserParser.InsertSelectContext context)
        {
            var formCommand = GetStatement(context.formParam());
            var fieldCommand = GetStatement(context.fieldElements());
            var selectCommand = _parserCommand.BracketCommand(GetStatement(context.selectStatement()));
            SetStatement(context, _parserCommand.InsertSelectCommand(formCommand, fieldCommand, selectCommand));
        }
        #endregion

        #region where statement define
        /// <summary>
        /// where bracket
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereBracket([NotNull] LinqParserParser.WhereBracketContext context)
        {
            SetStatement(context, _parserCommand.BracketCommand(GetStatement(context.whereStatement())));
        }

        /// <summary>
        /// where not
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereNot([NotNull] LinqParserParser.WhereNotContext context)
        {
            SetStatement(context, _parserCommand.NotCommand(GetStatement(context.whereStatement())));
        }

        /// <summary>
        /// where contains
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereContains([NotNull] LinqParserParser.WhereContainsContext context)
        {
            SetStatement(context, _parserCommand.InCommand(
                GetStatement(context.fieldParam()),
                GetStatement(context.inParameters())));
        }

        /// <summary>
        /// where like
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereLike([NotNull] LinqParserParser.WhereLikeContext context)
        {
            SetStatement(context, _parserCommand.LikeCommand(
                GetStatement(context.fieldParam()),
                 GetStatement(context.likeParam())));
        }

        /// <summary>
        /// where compare
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereCompare([NotNull] LinqParserParser.WhereCompareContext context)
        {
            var left = GetStatement(context.whereStatement(0));
            var right = GetStatement(context.whereStatement(1));
            var op = context.op.Text;
            if (right.ToUpper().Equals("NULL"))
            {
                if (op.Equals("="))
                {
                    SetStatement(context, _parserCommand.NullEqualCommand(left));
                    return;
                }
                SetStatement(context, _parserCommand.NullNotEqualCommand(left));
                return;
            }
            SetStatement(context, _parserCommand.WhereOperatorCommand(left, op, right));
        }

        /// <summary>
        /// where select compare
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereSelectCompare([NotNull] LinqParserParser.WhereSelectCompareContext context)
        {
            SetStatement(context, _parserCommand.WhereSelectCompareCommand(
                GetStatement(context.whereStatement()),
                context.op.Text,
                GetStatement(context.selectStatement())));
        }

        /// <summary>
        /// where select in
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereSelectIn([NotNull] LinqParserParser.WhereSelectInContext context)
        {
            SetStatement(context, _parserCommand.WhereSelectInCommand(
                GetStatement(context.fieldParam()),
                 GetStatement(context.selectStatement())));
        }

        /// <summary>
        /// where combination
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereCombination([NotNull] LinqParserParser.WhereCombinationContext context)
        {
            var opText = context.op.Text.ToUpper();
            if (opText.Equals("&&"))
            {
                opText = _parserCommand.AndCommand;
            }
            else if (opText.Equals("||"))
            {
                opText = _parserCommand.OrCommand;
            }
            SetStatement(context, _parserCommand.WhereOperatorCommand(GetStatement(context.whereStatement(0)),
                    opText,
                    GetStatement(context.whereStatement(1))));
        }

        /// <summary>
        /// where calculation
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereCalculation([NotNull] LinqParserParser.WhereCalculationContext context)
        {
            SetStatement(context, _parserCommand.WhereOperatorCommand(
                GetStatement(context.whereStatement(0)),
                context.op.Text,
                GetStatement(context.whereStatement(1))));
        }

        /// <summary>
        /// where extend method
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereExtend([NotNull] LinqParserParser.WhereExtendContext context)
        {
            SetStatement(context, GetStatement(context.extendMethod()));
        }

        /// <summary>
        /// where param
        /// </summary>
        /// <param name="context"></param>
        public override void ExitWhereParam([NotNull] LinqParserParser.WhereParamContext context)
        {
            SetStatement(context, GetStatement(context.parameter()));
        }
        #endregion

        #region element define
        /// <summary>
        /// set elements
        /// </summary>
        /// <param name="context"></param>
        public override void ExitUpdateSetElements([NotNull] LinqParserParser.UpdateSetElementsContext context)
        {
            var elementList = new List<string>();
            foreach (var child in context.updateSetElement())
            {
                elementList.Add(GetStatement(child));
            }
            SetStatement(context, _parserCommand.JoinSet(elementList));
        }

        /// <summary>
        /// set element
        /// </summary>
        /// <param name="context"></param>
        public override void ExitUpdateSetElement([NotNull] LinqParserParser.UpdateSetElementContext context)
        {
            var fieldCommand = GetStatement(context.fieldParam());
            var paramCommand = GetStatement(context.parameter());
            SetStatement(context, _parserCommand.EqualCommand(fieldCommand, paramCommand));
        }

        /// <summary>
        /// insert params
        /// </summary>
        /// <param name="context"></param>
        public override void ExitInsertParamElements([NotNull] LinqParserParser.InsertParamElementsContext context)
        {
            var elementList = new List<string>();
            foreach (var child in context.parameter())
            {
                elementList.Add(GetStatement(child));
            }
            SetStatement(context, _parserCommand.JoinSet(elementList));
        }

        /// <summary>
        /// order element
        /// </summary>
        /// <param name="context"></param>
        public override void ExitOrderElement([NotNull] LinqParserParser.OrderElementContext context)
        {
            var direction = " ";
            if (context.ORDERDIRECTION() != null)
            {
                direction = context.ORDERDIRECTION().GetText().ToUpper();
            }
            SetStatement(context,
                _parserCommand.OrderElementCommand(GetStatement(context.fieldParam()), direction));
        }

        /// <summary>
        /// join left define
        /// </summary>
        /// <param name="context"></param>
        public override void ExitJoinLeftElements([NotNull] LinqParserParser.JoinLeftElementsContext context)
        {
            SetStatement(context, GetStatement(context.fieldElements()));
        }

        /// <summary>
        /// join right define
        /// </summary>
        /// <param name="context"></param>
        public override void ExitJoinRightElements([NotNull] LinqParserParser.JoinRightElementsContext context)
        {
            SetStatement(context, GetStatement(context.fieldElements()));
        }

        /// <summary>
        /// field elements
        /// </summary>
        /// <param name="context"></param>
        public override void ExitFieldElements([NotNull] LinqParserParser.FieldElementsContext context)
        {
            var elementList = new List<string>();
            foreach (var child in context.fieldParam())
            {
                elementList.Add(GetStatement(child));
            }
            SetStatement(context, _parserCommand.JoinField(elementList));
        }

        /// <summary>
        /// select elements define
        /// </summary>
        /// <param name="context"></param>
        public override void ExitSelectElements([NotNull] LinqParserParser.SelectElementsContext context)
        {
            var elementList = new List<string>();
            foreach (var child in context.selectAsElement())
            {
                elementList.Add(GetStatement(child));
            }
            SetStatement(context, _parserCommand.JoinSelect(elementList));
        }

        /// <summary>
        /// select as element define
        /// </summary>
        /// <param name="context"></param>
        public override void ExitSelectFieldElement([NotNull] LinqParserParser.SelectFieldElementContext context)
        {
            var fieldKey = context.FIELDKEY().GetText().Trim();

            var column = AppConfigExtend.GetColumn(fieldKey);
            var command = string.Empty;
            if (context.TEXT() == null || string.IsNullOrWhiteSpace(context.TEXT().GetText()))
            {
                command = AppConfigExtend.GetSqlFullName(fieldKey);
            }
            else
            {
                command = string.Format("{0}.[{1}]", context.TEXT().GetText().Trim(), column);
            }

            if (context.LBRACKET() != null && context.RBRACKET() != null)
            {
                SetStatement(context, _parserCommand.RenameCommand(command, fieldKey));
                return;
            }
            SetStatement(context, command);
        }

        /// <summary>
        /// select element rename
        /// </summary>
        /// <param name="context"></param>
        public override void ExitSelectElementRename([NotNull] LinqParserParser.SelectElementRenameContext context)
        {
            var objectKey = context.TEXT().GetText();
            var elementCommand = GetStatement(context.selectElement());
            SetStatement(context, _parserCommand.RenameCommand(
               elementCommand,
                objectKey));
            var type = typeof(string);
            if (elementCommand.Contains(_parserCommand.AvgCommandKey) ||
                elementCommand.Contains(_parserCommand.SumCommandKey))
            {
                type = typeof(decimal?);
            }
            else if (elementCommand.Contains(_parserCommand.CountCommandKey))
            {
                type = typeof(int?);
            }
            if (!_generateObjects.Any(s => s.ObjectKey.ToUpper() == objectKey.ToUpper()))
            {
                _generateObjects.Add(new DynamicGenerateObject
                {
                    ObjectKey = objectKey,
                    CodeType = type
                });
            }
        }

        /// <summary>
        /// select text element
        /// </summary>
        /// <param name="context"></param>
        public override void ExitSelectTextElement([NotNull] LinqParserParser.SelectTextElementContext context)
        {
            SetStatement(context, context.GetText());
        }

        /// <summary>
        /// select element define
        /// </summary>
        /// <param name="context"></param>
        public override void ExitSelectElement([NotNull] LinqParserParser.SelectElementContext context)
        {
            if (context.getDateMethod() != null)
            {
                SetStatement(context, GetStatement(context.getDateMethod()));
                return;
            }
            if (context.getIdMethod() != null)
            {
                SetStatement(context, GetStatement(context.getIdMethod()));
                return;
            }
            if (context.getEmpidMethod() != null)
            {
                SetStatement(context, GetStatement(context.getEmpidMethod()));
                return;
            }
            if (context.getDeptidMethod() != null)
            {
                SetStatement(context, GetStatement(context.getDeptidMethod()));
                return;
            }
            if (context.sumMethod() != null)
            {
                SetStatement(context, GetStatement(context.sumMethod()));
                return;
            }
            if (context.avgMethod() != null)
            {
                SetStatement(context, GetStatement(context.avgMethod()));
                return;
            }
            SetStatement(context, GetStatement(context.countMethod()));
        }
        #endregion

        #region paramter define
        /// <summary>
        /// full param define
        /// </summary>
        /// <param name="context"></param>
        public override void ExitParameter([NotNull] LinqParserParser.ParameterContext context)
        {
            if (context.fieldParam() != null)
            {
                SetStatement(context, GetStatement(context.fieldParam()));
                return;
            }
            if (context.numberParam() != null)
            {
                SetStatement(context, GetStatement(context.numberParam()));
                return;
            }
            if (context.stringParam() != null)
            {
                SetStatement(context, GetStatement(context.stringParam()));
                return;
            }
            if (context.likeParam() != null)
            {
                SetStatement(context, GetStatement(context.likeParam()));
                return;
            }
            if (context.dateParam() != null)
            {
                SetStatement(context, GetStatement(context.dateParam()));
                return;
            }
            if (context.intParam() != null)
            {
                SetStatement(context, GetStatement(context.intParam()));
                return;
            }
            if (context.nullParam() != null)
            {
                SetStatement(context, GetStatement(context.nullParam()));
                return;
            }
            if (context.getDateMethod() != null)
            {
                SetStatement(context, GetStatement(context.getDateMethod()));
                return;
            }
            if (context.getIdMethod() != null)
            {
                SetStatement(context, GetStatement(context.getIdMethod()));
                return;
            }
            if (context.getEmpidMethod() != null)
            {
                SetStatement(context, GetStatement(context.getEmpidMethod()));
                return;
            }
            if (context.getDeptidMethod() != null)
            {
                SetStatement(context, GetStatement(context.getDeptidMethod()));
                return;
            }
            if (context.datePartMethod() != null)
            {
                SetStatement(context, GetStatement(context.datePartMethod()));
                return;
            }
            if (context.dateAddMethod() != null)
            {
                SetStatement(context, GetStatement(context.dateAddMethod()));
                return;
            }
            SetStatement(context, GetStatement(context.sqlParam()));
        }

        /// <summary>
        /// date param define
        /// </summary>
        /// <param name="context"></param>
        public override void ExitDateMethodParam([NotNull] LinqParserParser.DateMethodParamContext context)
        {
            if (context.dateParam() != null)
            {
                SetStatement(context, GetStatement(context.dateParam()));
                return;
            }
            if (context.getDateMethod() != null)
            {
                SetStatement(context, GetStatement(context.getDateMethod()));
                return;
            }
            SetStatement(context, GetStatement(context.fieldParam()));
        }

        /// <summary>
        /// validate date param define
        /// </summary>
        /// <param name="context"></param>
        public override void ExitValidateDateParam([NotNull] LinqParserParser.ValidateDateParamContext context)
        {
            if (context.dateMethodParam() != null)
            {
                SetStatement(context, GetStatement(context.dateMethodParam()));
                return;
            }
            SetStatement(context, GetStatement(context.dateAddMethod()));
        }

        /// <summary>
        /// in parameters define
        /// </summary>
        /// <param name="context"></param>
        public override void ExitInParameters([NotNull] LinqParserParser.InParametersContext context)
        {
            var paramKeyList = new List<string>();
            foreach (var inCtx in context.inParameter())
            {
                paramKeyList.Add(GetStatement(inCtx));
            }
            SetStatement(context, _parserCommand.JoinParam(paramKeyList));
        }

        /// <summary>
        /// in param define
        /// </summary>
        /// <param name="context"></param>
        public override void ExitInParameter([NotNull] LinqParserParser.InParameterContext context)
        {
            if (context.stringParam() != null)
            {
                SetStatement(context, GetStatement(context.stringParam()));
                return;
            }
            SetStatement(context, GetStatement(context.numberParam()));
        }
        #endregion

        #region method define
        /// <summary>
        /// extend methods
        /// </summary>
        /// <param name="context"></param>
        public override void ExitExtendMethod([NotNull] LinqParserParser.ExtendMethodContext context)
        {
            if (context.dateValidateMethod() != null)
            {
                SetStatement(context, GetStatement(context.dateValidateMethod()));
                return;
            }

            if (context.checkSingleMethod() != null)
            {
                SetStatement(context, GetStatement(context.checkSingleMethod()));
                return;
            }
            SetStatement(context, GetStatement(context.checkMultiMethod()));
        }

        /// <summary>
        /// 获取当前系统日期
        /// </summary>
        /// <param name="context"></param>
        public override void ExitGetDateMethod([NotNull] LinqParserParser.GetDateMethodContext context)
        {
            SetStatement(context, _parserCommand.DateCommand);
        }

        /// <summary>
        /// 获取一个新的GUID
        /// </summary>
        /// <param name="context"></param>
        public override void ExitGetIdMethod([NotNull] LinqParserParser.GetIdMethodContext context)
        {
            SetStatement(context, _parserCommand.NewId);
        }

        /// <summary>
        /// 获取当前上下文的人员id
        /// </summary>
        /// <param name="context"></param>
        public override void ExitGetEmpidMethod([NotNull] LinqParserParser.GetEmpidMethodContext context)
        {
            SetStatement(context, GetParamCommandByValue(_parserRequest.Current.EmplId));
        }

        /// <summary>
        /// 求和函数
        /// </summary>
        /// <param name="context"></param>
        public override void ExitSumMethod([NotNull] LinqParserParser.SumMethodContext context)
        {
            SetStatement(context, _parserCommand.SumCommand(GetStatement(context.fieldParam())));
        }

        /// <summary>
        /// 求条数函数
        /// </summary>
        /// <param name="context"></param>
        public override void ExitCountMethod([NotNull] LinqParserParser.CountMethodContext context)
        {
            var field = "1";
            if (context.fieldParam() != null)
            {
                field = GetStatement(context.fieldParam());
            }
            SetStatement(context, _parserCommand.CountCommand(field));
        }

        /// <summary>
        /// 求平均函数
        /// </summary>
        /// <param name="context"></param>
        public override void ExitAvgMethod([NotNull] LinqParserParser.AvgMethodContext context)
        {
            SetStatement(context, _parserCommand.AvgCommand(GetStatement(context.fieldParam())));
        }

        /// <summary>
        /// 获取当前上下文的部门id
        /// </summary>
        /// <param name="context"></param>
        public override void ExitGetDeptidMethod([NotNull] LinqParserParser.GetDeptidMethodContext context)
        {
            SetStatement(context, GetParamCommandByValue(_parserRequest.Current.DeptId));
        }

        /// <summary>
        /// 验证日期是否（年，季度，月份，天数）内
        /// </summary>
        /// <param name="context"></param>
        public override void ExitDateValidateMethod([NotNull] LinqParserParser.DateValidateMethodContext context)
        {
            SetStatement(context, _parserCommand.DateValidateCommand(
                context.DATEINTERVAL().GetText(),
                GetStatement(context.validateDateParam(0)),
                GetStatement(context.validateDateParam(1))));
        }

        /// <summary>
        /// 日期追加
        /// </summary>
        /// <param name="context"></param>
        public override void ExitDateAddMethod([NotNull] LinqParserParser.DateAddMethodContext context)
        {
            SetStatement(context, _parserCommand.DateAddCommand(
                context.DATEINTERVAL().GetText(),
                GetStatement(context.dateMethodParam()),
                GetParamCommandByValue(context.numberParam())));
        }

        /// <summary>
        /// 获取日期内容
        /// </summary>
        /// <param name="context"></param>
        public override void ExitDatePartMethod([NotNull] LinqParserParser.DatePartMethodContext context)
        {
            SetStatement(context, _parserCommand.DatePartCommand(
                context.DATEINTERVAL().GetText(),
                GetStatement(context.dateMethodParam())));
        }

        /// <summary>
        /// 判断多数据通过分割(分割列)
        /// </summary>
        /// <param name="context"></param>
        public override void ExitCheckMultiMethod([NotNull] LinqParserParser.CheckMultiMethodContext context)
        {
            SetStatement(context, _parserCommand.CheckMulitDataBySpitCommand(
                GetStatement(context.fieldParam()),
                GetStatement(context.stringParam())));
        }

        /// <summary>
        /// 判断多数据通过分割(单列)
        /// </summary>
        /// <param name="context"></param>
        public override void ExitCheckSingleMethod([NotNull] LinqParserParser.CheckSingleMethodContext context)
        {
            SetStatement(context, _parserCommand.CheckSingleDataBySpitCommand(
                GetStatement(context.fieldParam()),
                GetStatement(context.checkSingleParam())));
        }
        #endregion

        #region element param define
        /// <summary>
        /// stringParam
        /// </summary>
        /// <param name="context"></param>
        public override void ExitStringParam([NotNull] LinqParserParser.StringParamContext context)
        {
            // 当前DSL配置的字符串是用双引号来标记，因此字符串中包含"，则会写为 \"的形式
            // 但是sql中字符串是单引号标记的，包含"的时候不需要加\
            // 此处将操作将输入的 \" 转化为 "
            var text = context.GetText().Replace("\\\'", "\'").Replace("\'", "'");
            text = text.Substring(0, text.Length - 1);
            text = text.Substring(1);
            SetStatement(context, GetParamCommandByValue(text));
        }

        /// <summary>
        /// numberParam
        /// </summary>
        /// <param name="context"></param>
        public override void ExitNumberParam([NotNull] LinqParserParser.NumberParamContext context)
        {
            var text = context.GetText();
            var value = text.To<decimal>();
            SetStatement(context, GetParamCommandByValue(value));
        }

        /// <summary>
        /// intParam
        /// </summary>
        /// <param name="context"></param>
        public override void ExitIntParam([NotNull] LinqParserParser.IntParamContext context)
        {
            var text = context.GetText();
            var value = text.To<int>();
            SetStatement(context, GetParamCommandByValue(value));
        }

        /// <summary>
        /// likeParam
        /// </summary>
        /// <param name="context"></param>
        public override void ExitLikeParam([NotNull] LinqParserParser.LikeParamContext context)
        {
            var text = context.GetText().Replace("\\\'", "\'").Replace("\'", "'");
            text = text.Substring(0, text.Length - 1);
            text = text.Substring(1);
            var value = _parserCommand.LikeValueCommand(text);
            SetStatement(context, GetParamCommandByValue(value));
        }

        /// <summary>
        /// dateParam
        /// </summary>
        /// <param name="context"></param>
        public override void ExitDateParam([NotNull] LinqParserParser.DateParamContext context)
        {
            var text = context.GetText().Replace("T", " ");
            var value = text.To<DateTime>();
            SetStatement(context, GetParamCommandByValue(value));
        }

        /// <summary>
        /// nullParam
        /// </summary>
        /// <param name="context"></param>
        public override void ExitNullParam([NotNull] LinqParserParser.NullParamContext context)
        {
            SetStatement(context, context.GetText().Trim());
        }

        /// <summary>
        /// as param
        /// </summary>
        /// <param name="context"></param>
        public override void ExitAsParam([NotNull] LinqParserParser.AsParamContext context)
        {
            SetStatement(context, context.GetText().Trim());
        }

        /// <summary>
        /// sql param
        /// </summary>
        /// <param name="context"></param>
        public override void ExitSqlParam([NotNull] LinqParserParser.SqlParamContext context)
        {
            SetStatement(context, context.GetText().Trim());
        }

        /// <summary>
        /// check_singleParam
        /// </summary>
        /// <param name="context"></param>
        public override void ExitCheckSingleParam([NotNull] LinqParserParser.CheckSingleParamContext context)
        {
            var text = context.GetText().Replace("\\\'", "\'").Replace("\'", "'");
            var value = _parserCommand.CheckSingleValueCommand(text);
            SetStatement(context, GetParamCommandByValue(value));
        }

        /// <summary>
        /// formParam
        /// </summary>
        /// <param name="context"></param>
        public override void ExitFormParam([NotNull] LinqParserParser.FormParamContext context)
        {
            SetStatement(context, context.GetText().Trim());
        }

        /// <summary>
        /// fieldParam
        /// </summary>
        /// <param name="context"></param>
        public override void ExitFieldParam([NotNull] LinqParserParser.FieldParamContext context)
        {
            var fieldKey = context.FIELDKEY().GetText().Trim();

            var column = AppConfigExtend.GetColumn(fieldKey);
            var command = string.Empty;
            if (context.TEXT() == null || string.IsNullOrWhiteSpace(context.TEXT().GetText()))
            {
                command = AppConfigExtend.GetSqlFullName(fieldKey);
                SetStatement(context, command);
                return;
            }

            command = string.Format("{0}.[{1}]", context.TEXT().GetText().Trim(), column);
            SetStatement(context, command);
        }
        #endregion
        #endregion
    }
}
