grammar BudgetCli;

@members { public bool strict { get; set; } = false; }

expression: command
          ;

command: HELP                                               #help
       | VERSION                                            #version
       | UNDO                                               #undo
       | REDO                                               #redo

       //ACCOUNTS
       | CAT ACCOUNT (optHelp | (string optDate?))                  #catAccounts
       | LS ACCOUNT                                                 #listAccounts
       | NEW ACCOUNT (optHelp | optionsListAccountLs)               #newAccount
       | REMOVE ACCOUNT (optHelp | optRecursive?)                   #removeAccount
       | MOVE ACCOUNT (optHelp | (oldName=string newName=string))   #moveAccount
       | SET ACCOUNT (optHelp | optionsListAccountSet)              #setAccount
       
       //TRANSACTIONS
       | CAT TRANSACTION (optHelp | id=integer)                         #catTransaction
       | LS TRANSACTION (optHelp | optionsListTransactionLs)            #listTransactions
       | NEW TRANSACTION (optHelp | optionsListTransactionNew)          #newTransaction

       //REPORTING
       | LS HISTORY (optHelp | optionsListHistoryLs)                    #listHistory
       ;

//Options

optionsListAccountLs
locals [int categoryCount = 0,
        int descriptionCount = 0,
        int fundsCount = 0,
        int nameCount = 0,
        int priorityCount = 0,
        int typeCount = 0]
            :(
                {!strict || $categoryCount < 1}? optName {$categoryCount++;} |
                {!strict || $descriptionCount < 1}? optDescription {$descriptionCount++;} |
                {!strict || $fundsCount < 1}? optFundsExpr {$fundsCount++;} |
                {!strict || $nameCount < 1}? optName {$nameCount++;} |
                {!strict || $priorityCount < 1}? optPriority {$priorityCount++;} |
                {!strict || $typeCount < 1}? optAccountType {$typeCount++;}
             )*
            ;

optionsListAccountNew
locals [int categoryCount = 0,
        int descriptionCount = 0,
        int fundsCount = 0,
        int priorityCount = 0,
        int typeCount = 0]
            :(
                {!strict || $categoryCount < 1}? optName {$categoryCount++;} |
                {!strict || $descriptionCount < 1}? optDescription {$descriptionCount++;} |
                {!strict || $fundsCount < 1}? optFunds {$fundsCount++;} |
                {!strict || $priorityCount < 1}? optPriority {$priorityCount++;} |
                {!strict || $typeCount < 1}? optAccountType {$typeCount++;}
             )*
            ;

optionsListAccountSet
locals [int categoryCount = 0,
        int descriptionCount = 0,
        int fundsCount = 0,
        int nameCount = 0,
        int priorityCount = 0,
        int typeCount = 0]
            :(
                {!strict || $categoryCount < 1}? optName {$categoryCount++;} |
                {!strict || $descriptionCount < 1}? optDescription {$descriptionCount++;} |
                {!strict || $fundsCount < 1}? optFunds {$fundsCount++;} |
                {!strict || $nameCount < 1}? optName {$nameCount++;} |
                {!strict || $priorityCount < 1}? optPriority {$priorityCount++;} |
                {!strict || $typeCount < 1}? optAccountType {$typeCount++;}
             )*
            ;

optionsListTransactionLs
locals[int accountCount = 0,
       int dateExprCount = 0,
       int fundsCount = 0,
       int idCount = 0,
       int typeCount = 0]
           :(
               {!strict || $accountCount < 1}? optAccount {$accountCount++;} |
               {!strict || $dateExprCount < 1}? optDateExpr {$dateExprCount++;} |
               {!strict || $fundsCount < 1}? optFundsExpr {$fundsCount++;} |
               {!strict || $idCount < 1}? optId {$idCount++;} |
               {!strict || $typeCount < 1}? optTransactionType {$typeCount++;}
            )*
           ;

optionsListTransactionNew
locals [int fundsCount = 0,
        int sourceCount = 0,
        int destCount = 0]
            :(
                {!strict || $sourceCount < 1}? optSource {$sourceCount++;} |
                {!strict || $destCount < 1}? optDest {$destCount++;} |
                {!strict || $fundsCount < 1}? optFunds {$fundsCount++;}
             )*
            ;

optionsListHistoryLs
locals [int countCount = 0,
        int dateExprCount = 0]
            :(
                {!strict || $countCount < 1}? optCount {$countCount++;} |
                {!strict || $dateExprCount < 1}? optDateExpr {$dateExprCount++;}
             )*
            ;



optAccount: ('-a' | '--account') accountName=string;
optAmountExpr: ('-a' | '--amount') decimalExpr;
optCategory: ('-c' | '--category') categoryName=string;
optCount: ('-c' | '--count') integer;
optDate: ('-d' | '--date') date;
optDateExpr: ('-d' | '--date') dateExpr;
optDescription: ('-d' | '--description') string;
optDest: ('-d' | '--destination') destination=string;
optFunds: ('-f' | '--funds') decimal;
optFundsExpr: ('-f' | '--funds') decimalExpr;
optHelp: '-h' | '--help';
optId: ('-i' | '--id') integer;
optName: ('-n' | '--name') name=string;
optPriority: ('-p' | '--priority') priority=intExpr;
optRecursive: ('-r' | '--recurseive');
optSource: ('-s' | '--source') source=string;
optAccountType
locals [BudgetCliData.Enums.AccountKind kind]
            : CATEGORY      { $kind = BudgetCliData.Enums.AccountKind.Category; }
            | SOURCE        { $kind = BudgetCliData.Enums.AccountKind.Source; }
            | SINK          { $kind = BudgetCliData.Enums.AccountKind.Sink; }
            ;
optTransactionType
locals [BudgetCliCore.Enums.TransactionKind kind]
            : ('-t' | '--type')
              (
                INFLOW {$kind = BudgetCliCore.Enums.TransactionKind.Inflow;} |
                OUTFLOW {$kind = BudgetCliCore.Enums.TransactionKind.Outflow;} |
                INTERNAL {$kind = BudgetCliCore.Enums.TransactionKind.Internal;} |
              )
            ;



intExpr
returns [BudgetCliCore.Models.Range<int> range]
            : from=integer ':' to=integer       {$range = new BudgetCliCore.Models.Range<int>($from.value, $to.value);}
            | integer (PLUS { $range = new BudgetCliCore.Models.Range<int>($integer.value, int.MaxValue); } | MINUS { $range = new BudgetCliCore.Models.Range<int>(int.MinValue, $integer.value); })
            ;

decimalExpr
returns [BudgetCliCore.Models.Range<decimal> range]
            : from=decimal ':' to=decimal       {$range = new BudgetCliCore.Models.Range<decimal>($from.value, $to.value);}
            | decimal (PLUS { $range = new BudgetCliCore.Models.Range<decimal>($decimal.value, decimal.MaxValue); } | MINUS { $range = new BudgetCliCore.Models.Range<decimal>(decimal.MinValue, $decimal.value); })
            ;

dateExpr: date
        | from=date PLUS
        | to=date MINUS
        | from=date ':' to=date
        ;

//Types & Expressions



integer returns [int value]: CURRENCY? '(' CURRENCY? DIGITS ')'     { $value = -$DIGITS.int; }
                           | CURRENCY? '-' CURRENCY? DIGITS         { $value = -$DIGITS.int; }
                           | CURRENCY? DIGITS                       { $value = $DIGITS.int; }
                           ;

decimal returns [decimal value]: integer                                                        { $value = (decimal) $integer.value; }
                               | CURRENCY? '(' CURRENCY? whole=DIGITS '.' fraction=DIGITS ')'   { $value = -decimal.Parse($"{$whole.int}.{$fraction.int}"); }
                               | CURRENCY? '-' CURRENCY? whole=DIGITS '.' fraction=DIGITS       { $value = -decimal.Parse($"{$whole.int}.{$fraction.int}"); }
                               | CURRENCY? CURRENCY? whole=DIGITS '.' fraction=DIGITS           { $value = decimal.Parse($"{$whole.int}.{$fraction.int}"); }
                               ;

date: DATE_DAY ('.' | '/' | '-') DATE_MONTH (('.' | '/' | '-') DATE_YEAR)?  #explicitDate
    | LAST dayOfWeek                                                        #relativeDayOfWeekDate
    | LAST month DATE_DAY                                                   #relativeDayOfMonthDate
    | DIGITS timeUnit AGO                                                   #relativeDate
    ;



month returns [BudgetCliCore.Enums.Month Month]: JANUARY        { $Month = BudgetCliCore.Enums.Month.January; }
                                               | FEBRUARY       { $Month = BudgetCliCore.Enums.Month.February; }
                                               | MARCH          { $Month = BudgetCliCore.Enums.Month.March; }
                                               | APRIL          { $Month = BudgetCliCore.Enums.Month.April; }
                                               | MAY            { $Month = BudgetCliCore.Enums.Month.May; }
                                               | JUNE           { $Month = BudgetCliCore.Enums.Month.June; }
                                               | JULY           { $Month = BudgetCliCore.Enums.Month.July; }
                                               | AUGUST         { $Month = BudgetCliCore.Enums.Month.August; }
                                               | SEPTEMBER      { $Month = BudgetCliCore.Enums.Month.September; }
                                               | OCTOBER        { $Month = BudgetCliCore.Enums.Month.October; }
                                               | NOVEMBER       { $Month = BudgetCliCore.Enums.Month.November; }
                                               | DECEMBER       { $Month = BudgetCliCore.Enums.Month.December; }
                                               ;

timeUnit returns [BudgetCliCore.Enums.TimeUnit TimeUnit]: UNIT_DAY    { $TimeUnit = BudgetCliCore.Enums.TimeUnit.Day; }
                                                        | UNIT_WEEK   { $TimeUnit = BudgetCliCore.Enums.TimeUnit.Week; }
                                                        | UNIT_MONTH  { $TimeUnit = BudgetCliCore.Enums.TimeUnit.Month; }
                                                        | UNIT_YEAR   { $TimeUnit = BudgetCliCore.Enums.TimeUnit.Year; }
                                                        ;

dayOfWeek returns [System.DayOfWeek Day]: MONDAY       { $Day = System.DayOfWeek.Monday; }
                                        | TUESDAY      { $Day = System.DayOfWeek.Tuesday; }
                                        | WEDNESDAY    { $Day = System.DayOfWeek.Wednesday; }
                                        | THURSDAY     { $Day = System.DayOfWeek.Thursday; }
                                        | FRIDAY       { $Day = System.DayOfWeek.Friday; }
                                        | SATURDAY     { $Day = System.DayOfWeek.Saturday; }
                                        | SUNDAY       { $Day = System.DayOfWeek.Sunday; }
                                        ;

string returns [string Text]: '"' (CHAR|' ') '"'     { $Text = $text.Substring(1,$text.Length-2); /* Trim off quote marks */ }
                            | CHAR+                  { $Text = $text; }
                            ;

// LEXER RULES
PLUS: '+';
MINUS: '-';

HELP: H (E L P)?;
VERSION: V (E R S I O N)?;
UNDO: U N D O;
REDO: R E D O;

ACCOUNT: A (C C O U N T S?)?;
CATEGORY: C (A T E G O R Y)?;
SOURCE: S O U R C E;
SINK: S I N K;
TRANSACTION: T (R A N (S A C T I O N S?)?)?;
INFLOW: I N F L O W;
OUTFLOW: O U T F L O W;
INTERNAL: I N T E R N A L;

HISTORY: H (I S T O R Y)?;

CAT: C A T;
LS: L S
  | L I S T
  ;
MOVE: M V
    | M O V E
    ;
NEW: N E W;
REMOVE: R M
      | R E M O V E
      | D E L (E T E)?
      ;
SET: S E T;
ADD: A D D;
SUBTRACT: S U B (T R A C T)?;       
DATE_DAY: [012]? DIGIT;
DATE_MONTH: [01]? DIGIT;
DATE_YEAR: (DIGIT DIGIT)? DIGIT DIGIT;
LAST: L A S T;
AGO: A G O;

JANUARY: J A N (U A R Y)?;
FEBRUARY: F E B (U A R Y)?;
MARCH: M A R (C H)?;
APRIL: A P R (I L)?;
MAY: M A Y;
JUNE: J U N E?;
JULY: J U L Y?;
AUGUST: A U G (U S T)?;
SEPTEMBER: S E P (T E M B E R)?;
OCTOBER: O C T (O B E R)?;
NOVEMBER: N O V (E M B E R)?;
DECEMBER: D E C (E M B E R)?;

UNIT_DAY: D A Y S?
        | D S?
        ;

UNIT_WEEK: W E E K S?
         | W (K? S)?
         ;

UNIT_MONTH: M O N T H S?
          | M (O S?)?
          ;

UNIT_YEAR: Y E A R S?
         | Y (R S?)?
         ;
         
MONDAY   : M O N (D A Y)?;
TUESDAY  : T U E (S D A Y)?;
WEDNESDAY: W E D (N E S D A Y)?;
THURSDAY : T H U (R S D A Y)?;
FRIDAY   : F R I (D A Y)?;
SATURDAY : S A T (U R D A Y)?;
SUNDAY   : S U N (D A Y)?;

fragment A:('a'|'A');
fragment B:('b'|'B');
fragment C:('c'|'C');
fragment D:('d'|'D');
fragment E:('e'|'E');
fragment F:('f'|'F');
fragment G:('g'|'G');
fragment H:('h'|'H');
fragment I:('i'|'I');
fragment J:('j'|'J');
fragment K:('k'|'K');
fragment L:('l'|'L');
fragment M:('m'|'M');
fragment N:('n'|'N');
fragment O:('o'|'O');
fragment P:('p'|'P');
fragment Q:('q'|'Q');
fragment R:('r'|'R');
fragment S:('s'|'S');
fragment T:('t'|'T');
fragment U:('u'|'U');
fragment V:('v'|'V');
fragment W:('w'|'W');
fragment X:('x'|'X');
fragment Y:('y'|'Y');
fragment Z:('z'|'Z');

CURRENCY: '$';
DIGIT: [0-9];
DIGITS: DIGIT+;

CHAR: [a-zA-Z0-9_\-!@#$%^&*()/,.?<>'];
WS  : [ \t\r\n]+ -> skip ;