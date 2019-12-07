//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:\Users\masta\Documents\repos\Budget-CLI\BudgetCliCore\Grammar\BudgetCli.g4 by ANTLR 4.7.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace BudgetCliCore.Grammar {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="BudgetCliParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.1")]
[System.CLSCompliant(false)]
public interface IBudgetCliVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] BudgetCliParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>help</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHelp([NotNull] BudgetCliParser.HelpContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>version</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVersion([NotNull] BudgetCliParser.VersionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>undo</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUndo([NotNull] BudgetCliParser.UndoContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>redo</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRedo([NotNull] BudgetCliParser.RedoContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>catAccounts</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCatAccounts([NotNull] BudgetCliParser.CatAccountsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listAccounts</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListAccounts([NotNull] BudgetCliParser.ListAccountsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>newAccount</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNewAccount([NotNull] BudgetCliParser.NewAccountContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>removeAccount</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRemoveAccount([NotNull] BudgetCliParser.RemoveAccountContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>moveAccount</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMoveAccount([NotNull] BudgetCliParser.MoveAccountContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>setAccount</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSetAccount([NotNull] BudgetCliParser.SetAccountContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>catTransaction</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCatTransaction([NotNull] BudgetCliParser.CatTransactionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listTransactions</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListTransactions([NotNull] BudgetCliParser.ListTransactionsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>newTransaction</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNewTransaction([NotNull] BudgetCliParser.NewTransactionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listHistory</c>
	/// labeled alternative in <see cref="BudgetCliParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListHistory([NotNull] BudgetCliParser.ListHistoryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optionsListAccountLs"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptionsListAccountLs([NotNull] BudgetCliParser.OptionsListAccountLsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optionsListAccountNew"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptionsListAccountNew([NotNull] BudgetCliParser.OptionsListAccountNewContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optionsListAccountSet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptionsListAccountSet([NotNull] BudgetCliParser.OptionsListAccountSetContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optionsListTransactionLs"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptionsListTransactionLs([NotNull] BudgetCliParser.OptionsListTransactionLsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optionsListTransactionNew"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptionsListTransactionNew([NotNull] BudgetCliParser.OptionsListTransactionNewContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optionsListHistoryLs"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptionsListHistoryLs([NotNull] BudgetCliParser.OptionsListHistoryLsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optAccount"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptAccount([NotNull] BudgetCliParser.OptAccountContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optAmountExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptAmountExpr([NotNull] BudgetCliParser.OptAmountExprContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optCategory"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptCategory([NotNull] BudgetCliParser.OptCategoryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optCount"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptCount([NotNull] BudgetCliParser.OptCountContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optDate"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptDate([NotNull] BudgetCliParser.OptDateContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optDateExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptDateExpr([NotNull] BudgetCliParser.OptDateExprContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optDescription"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptDescription([NotNull] BudgetCliParser.OptDescriptionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optDest"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptDest([NotNull] BudgetCliParser.OptDestContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optFunds"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptFunds([NotNull] BudgetCliParser.OptFundsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optFundsExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptFundsExpr([NotNull] BudgetCliParser.OptFundsExprContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optHelp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptHelp([NotNull] BudgetCliParser.OptHelpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optId"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptId([NotNull] BudgetCliParser.OptIdContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptName([NotNull] BudgetCliParser.OptNameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optPriority"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptPriority([NotNull] BudgetCliParser.OptPriorityContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optRecursive"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptRecursive([NotNull] BudgetCliParser.OptRecursiveContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optSource"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptSource([NotNull] BudgetCliParser.OptSourceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optAccountType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptAccountType([NotNull] BudgetCliParser.OptAccountTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.optTransactionType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptTransactionType([NotNull] BudgetCliParser.OptTransactionTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.intExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIntExpr([NotNull] BudgetCliParser.IntExprContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.decimalExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDecimalExpr([NotNull] BudgetCliParser.DecimalExprContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.dateExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDateExpr([NotNull] BudgetCliParser.DateExprContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.integer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInteger([NotNull] BudgetCliParser.IntegerContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.decimal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDecimal([NotNull] BudgetCliParser.DecimalContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>explicitDate</c>
	/// labeled alternative in <see cref="BudgetCliParser.date"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExplicitDate([NotNull] BudgetCliParser.ExplicitDateContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>relativeDayOfWeekDate</c>
	/// labeled alternative in <see cref="BudgetCliParser.date"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRelativeDayOfWeekDate([NotNull] BudgetCliParser.RelativeDayOfWeekDateContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>relativeDayOfMonthDate</c>
	/// labeled alternative in <see cref="BudgetCliParser.date"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRelativeDayOfMonthDate([NotNull] BudgetCliParser.RelativeDayOfMonthDateContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>relativeDate</c>
	/// labeled alternative in <see cref="BudgetCliParser.date"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRelativeDate([NotNull] BudgetCliParser.RelativeDateContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.month"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMonth([NotNull] BudgetCliParser.MonthContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.timeUnit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTimeUnit([NotNull] BudgetCliParser.TimeUnitContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.dayOfWeek"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDayOfWeek([NotNull] BudgetCliParser.DayOfWeekContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="BudgetCliParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] BudgetCliParser.StringContext context);
}
} // namespace BudgetCliCore.Grammar
