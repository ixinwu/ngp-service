grammar LinqParser;

linqStatement
    : selectStatement
    | whereStatement
	| orderStatement
	| updateStatements
	| insertStatements
;

selectStatement
    : formStatement (formStatement)*
      (joinStatement)*
      (WHERE whereStatement)?
      (orderStatement (LIMIT INT OFFSET INT)?)?
      (groupStatement)?
      (DISTINCT)? SELECT (TOP INT)?  selectElements
;

formStatement
    : FROM asParam IN schemaStatement
;

joinStatement
    : (JOINDIRECTION)? JOIN asParam  IN schemaStatement
        ON joinLeftElements  EQUAL joinRightElements
;

schemaStatement
    : formParam
    | LPAREN selectStatement RPAREN
;

groupStatement
    : GROUP BY fieldElements
;

orderStatement
    : ORDER BY LBRACE orderElement (COMMA orderElement)* RBRACE
;

updateStatements
	: updateStatement SEMIC (updateStatement SEMIC)*
	| updateStatement
;

updateStatement
	: UPDATE formParam SET updateSetElements (WHERE whereStatement)?
;

insertStatements
	: insertStatement SEMIC (insertStatement SEMIC)*
	| insertStatement
;

insertStatement
	: INSERT  formParam LPAREN fieldElements RPAREN VALUES LPAREN insertParamElements RPAREN    #InsertValues
	| INSERT formParam LPAREN fieldElements RPAREN LPAREN selectStatement RPAREN                 #InsertSelect 
;

whereStatement
	:
	  LPAREN whereStatement  RPAREN													#WhereBracket
	| op = NOT whereStatement  																#WhereNot
	| fieldParam op = IN  LPAREN inParameters	RPAREN		                    #WhereContains
	| fieldParam op = LIKE likeParam															#WhereLike
	| whereStatement op = COMPARE whereStatement												#WhereCompare
	| whereStatement op = COMPARE LPAREN selectStatement RPAREN	           #WhereSelectCompare
	| fieldParam op = CONTAINS LPAREN selectStatement RPAREN					#WhereSelectIn
	| whereStatement op = LOGIC whereStatement												#WhereCombination
	| whereStatement op = CALCULATION whereStatement                                          #WhereCalculation
	| extendMethod                                                                           #WhereExtend
	| parameter													        					#WhereParam
;

/*
 * element define
 */
 updateSetElements
	: updateSetElement (COMMA updateSetElement)*
;
 updateSetElement
	: fieldParam op = COLON parameter
;

insertParamElements
	: parameter (COMMA parameter)*
;

orderElement
    : fieldParam (ORDERDIRECTION)?
;

joinLeftElements
    : LBRACE fieldElements RBRACE
;

joinRightElements
    : LBRACE fieldElements RBRACE
;

fieldElements
    : fieldParam (COMMA fieldParam)*
;

/*
 * select elements
 */
selectElements
    : LBRACE selectAsElement (COMMA selectAsElement)* RBRACE
;
selectAsElement
    : TEXT op = COLON selectElement                 #SelectElementRename
    | fieldParam                           #SelectFieldElement
;

selectElement
    : getDateMethod
    | getIdMethod
    | getEmpidMethod
    | getDeptidMethod
    | sumMethod
    | avgMethod
    | countMethod
;

/*
 * full param define
 */
parameter
	: fieldParam
	| numberParam
	| stringParam
	| likeParam
	| dateParam
	| intParam
	| nullParam
	| getDateMethod
	| getIdMethod
	| getEmpidMethod
	| getDeptidMethod
	| datePartMethod
	| dateAddMethod
	| sqlParam
;

/*
 * date param define
 */
dateMethodParam
	: dateParam
	| getDateMethod
	| fieldParam
;

/*
 * validate date param define
 */
validateDateParam
	: dateMethodParam
	| dateAddMethod
;

/*
 * in param define
 */
inParameters
    : inParameter (COMMA inParameter)*
;
inParameter
	: stringParam
	| numberParam
;

/*
 * method define
 */
extendMethod
    : dateValidateMethod
    | checkSingleMethod
    | checkMultiMethod
;

/*
 * 获取当前系统日期
 * 方法签名 getdate()
 */
getDateMethod
	: GETDATE  LPAREN RPAREN
;

/*
 * 获取一个新的GUID
 * 方法签名 getid()
 */
getIdMethod
	: GETID	 LPAREN RPAREN
;

/*
 * 获取当前上下文的人员id
 * 方法签名 getempid()
 */
getEmpidMethod
	: GETEMPID   LPAREN RPAREN
;

/*
 * 求和函数
 */
sumMethod
    : SUM LPAREN fieldParam RPAREN
;

/*
 * 求条数函数
 */
countMethod
    : COUNT LPAREN (fieldParam)? RPAREN
;

/*
 * 求平均函数
 */
avgMethod
    : AVG LPAREN fieldParam RPAREN
;

/*
 * 获取当前上下文的部门id
 * 方法签名 getdeptid()
 */
getDeptidMethod
	: GETDEPTID  LPAREN RPAREN
;

/*
 * 验证日期是否（年，季度，月份，天数）内
 * 方法签名 validatedate(type,comparedate,paramdate)
 * type:y:年；q：季度；m：月份；d:天数
 * comparedate : 待比较日期
 * paramdate : 参数日期
 */
dateValidateMethod
	: DATEVALIDATE LPAREN DATEINTERVAL COMMA validateDateParam COMMA validateDateParam RPAREN
;

/*
 * 日期追加
 * 方法签名 dateadd(type,,paramdate,count)
 * type:y:年；q：季度；m：月份；d:天数
 * paramdate : 参数日期
 * number:添加的内容
 */
dateAddMethod
	:DATEADD LPAREN DATEINTERVAL COMMA dateMethodParam COMMA  numberParam RPAREN
;

/*
 * 获取日期内容
 * 方法签名 datepart(type,,paramdate,count)
 * type:y:年；q：季度；m：月份；d:天数
 * paramdate : 参数日期
 */
datePartMethod
	:DATEPART LPAREN DATEINTERVAL COMMA dateMethodParam RPAREN
;

/*
 * 判断多数据通过分割(分割列)
 * fieldParam 字段参数
 * stringParam 字符串
 */
checkMultiMethod
	:CHECKMULIT LPAREN fieldParam COMMA stringParam RPAREN
;

/*
 * 判断多数据通过分割(单列)
 * fieldParam 字段参数
 * stringParam 字符串
 */
checkSingleMethod
	:CHECKSINGLE LPAREN fieldParam COMMA checkSingleParam RPAREN
;

/*
 * element param define
 */
stringParam : STRING;
numberParam : NUMBER;
intParam : INT;
likeParam : STRING;
dateParam : DATE;
nullParam : NULL;
asParam : TEXT;
sqlParam : ART TEXT;
checkSingleParam : STRING;
formParam : FORMKEY;
fieldParam : (TEXT POINT)? FIELDKEY;
/*
 * Key Word Lexer Definitions
 */
WHERE : [wW][hH][eE][rR][eE];
SUM : [sS][uU][mM];
GROUP : [gG][rR][oO][uU][pP];
BY : [bB][yY];
FROM : [fF][rR][oO][mM];
COUNT : [cC][oO][uU][nN][tT];
SELECT : [sS][eE][lL][eE][cC][tT];
IN : [iI][nN];
DISTINCT : [dD][iI][sS][tT][iI][nN][cC][tT];
CONTAINS : [cC][oO][nN][tT][aA][iI][nN][sS];
ON : [oO][nN];
AVG : [aA][vV][gG];
EQUAL : [eE][qQ][uU][aA][lL];
LIKE : [lL][iI][kK][eE];
JOIN : [jJ][oO][iI][nN];
JOINDIRECTION : [lL][eE][fF][tT] | [rR][iI][gG][hH][tT] | [iI][nN][nN][eE][rR];
ORDER : [oO][rR][dD][eE][rR];
ORDERDIRECTION : [aA][sS][cC] | [dD][eE][sS][cC];
LIMIT : [lL][iI][mM][iI][tT];
OFFSET : [oO][fF][fF][sS][eE][tT];
TOP : [tT][oO][pP];
UPDATE:[uU][pP][dD][aA][tT][eE];
DELETE : [dD][eE][lL][eE][tT][eE];
INSERT : [iI][nN][sS][eE][rR][tT];
SET : [sS][eE][tT];
VALUES : [vV][aA][lL][uU][eE][sS];

/*
 *  operator Lexer Definitions
 */
LPAREN : '(';
RPAREN : ')';
LBRACE :'{';
RBRACE :'}';
LBRACKET :'[';
RBRACKET :']';
COMMA : ',';
POINT :'.';
SEMIC :';';
UNDERLINE :'_';
COLON : ':';
CALCULATION :  '+' | '-' | '*' | '/';
COMPARE : '>' | '<' | '>=' | '<=' | '=' | '!=';
LOGIC : '&&' | '||' | [aA][nN][dD] | [oO][rR];
NOT : '!' | [nN][oO][tT];
ART : '@';

/*
 * System Method Lexer Definitions
 */
// 方法:获取当前时间
GETDATE
    : [gG][eE][tT][dD][aA][tT][eE]
;
// 方法:创建ID
GETID
    : [gG][eE][tT][iI][dD]
;
// 方法:获取当前人员id
GETEMPID
    : [gG][eE][tT][eE][mM][pP][iI][dD]
;
// 方法:获取当前部门id
GETDEPTID
    : [gG][eE][tT][dD][eE][pP][tT][iI][dD]
;

/*
 * Extend Lexer Definitions
 */
DATEINTERVAL
    : [yY][yY]
    | [qQ][qQ]
    | [mM][mM]
    | [wW][kK]
    | [dD][dD]
    | [dD][yY]
    | [dD][wW]
;
NULL : [nN][uU][lL][lL];
// 方法:日期验证
DATEVALIDATE
    : [dD][aA][tT][eE][vV][aA][lL][iI][dD][aA][tT][eE]
;
// 方法:日期运算
DATEADD
    : [dD][aA][tT][eE][aA][dD][dD]
;
// 方法:日期内容
DATEPART
    : [dD][aA][tT][eE][pP][aA][rR][tT]
;
// 方法:数据验证
DATEREPEAT
    : [dD][aA][tT][aA][rR][eE][pP][eE][aA][tT]
;
// 方法：判断多数据通过分割(分割列)
CHECKMULIT
    : [cC][hH][eE][cC][kK][mM][uU][lL][iI][tT]
;
// 方法：判断多数据通过分割(单列)
CHECKSINGLE
    : [cC][hH][eE][cC][kK][sS][iI][nN][gG][lL][eE]
;

/*
 * element Lexer Definitions
 */
TEXT
    : [a-zA-Z]([a-zA-Z] | [0-9])*
;
//字符串
STRING
    : '"'('\\'["\\/bfnrt] | ~["\\])*'"'
;
//表单Key
FORMKEY
	: TEXT UNDERLINE TEXT
;

//字段Key
FIELDKEY
    : FORMKEY UNDERLINE TEXT
;
//日期格式
DATE
    : DATENUM ('/'|'-') DATENUM ('/'|'-')  DATENUM ([tT]? DATENUM  ':' DATENUM ':' DATENUM)?
;
// double number
NUMBER
    : '-'? INT '.' [0-9]+ EXP? // 1.35, 1.35E-9, 0.3, -4.5
;
// leading zeros
INT
    :'-'? '0' | [1-9] [0-9]*
;
fragment DATENUM
    : [0-9]*
;
// \- since - means "range" inside [...]
fragment EXP
    : [Ee] [+\-]? INT
;

/*
*   comment Lexer Definitions
*/
WS : [ \t\r\n]+ -> skip ;