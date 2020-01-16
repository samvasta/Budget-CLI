using System.Reflection.Metadata.Ecma335;
using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using BudgetCli.Core.Grammar;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Commands.Accounts;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Models;
using BudgetCli.Core.Models.Commands.SystemCommands;
using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Interpreters.Visitors
{
    public class BudgetCliCommandVisitor : BudgetCliVisitorBase<InterpreterResult<ICommandAction>>
    {
        private readonly RepositoryBag _repositories;
        public BudgetCliCommandVisitor(RepositoryBag repositories)
        {
            _repositories = repositories;
        }

        #region - System Commands -

        public override InterpreterResult<ICommandAction> VisitExit(BudgetCliParser.ExitContext context)
        {
            return new InterpreterResult<ICommandAction>(new SystemCommand(context.GetText(), SystemCommandKind.Exit));
        }

        public override InterpreterResult<ICommandAction> VisitClear(BudgetCliParser.ClearContext context)
        {
            return new InterpreterResult<ICommandAction>(new SystemCommand(context.GetText(), SystemCommandKind.ClearConsole));
        }

        public override InterpreterResult<ICommandAction> VisitVersion(BudgetCliParser.VersionContext context)
        {
            return new InterpreterResult<ICommandAction>(new SystemCommand(context.GetText(), SystemCommandKind.Version));
        }

        public override InterpreterResult<ICommandAction> VisitHelp(BudgetCliParser.HelpContext context)
        {
            return new InterpreterResult<ICommandAction>(new HelpCommand(context.GetText(), _repositories, CommandActionKind.Help));
        }

        #endregion

        #region - Accounts -

        public override InterpreterResult<ICommandAction> VisitDetailAccounts([NotNull] BudgetCliParser.DetailAccountsContext context)
        {
            throw new NotImplementedException();
        }

        public override InterpreterResult<ICommandAction> VisitListAccounts([NotNull] BudgetCliParser.ListAccountsContext context)
        {
            var helpOption = context.optHelp();
            if(helpOption != null)
            {
                var helpCommand = new HelpCommand(context.GetText(), _repositories, Data.Enums.CommandActionKind.ListAccount);
                return new InterpreterResult<ICommandAction>(helpCommand);
            }

            
            var optionsListContext = context.optionsListAccountLs();
            ListAccountCommand command = new ListAccountCommand(context.GetText(), _repositories);

            var idOptionContext = optionsListContext.optId().FirstOrDefault();
            if(idOptionContext != null)
            {
                command.IdOption.SetData(idOptionContext.id.value);
            }

            var nameOptionContext = optionsListContext.optName().FirstOrDefault();
            if(nameOptionContext != null)
            {
                command.NameOption.SetData(nameOptionContext.name.Text);
            }
            
            var descriptionOptionContext = optionsListContext.optDescription().FirstOrDefault();
            if(descriptionOptionContext != null)
            {
                command.DescriptionOption.SetData(descriptionOptionContext.description.Text);
            }

            var accountTypeOptionContext = optionsListContext.optAccountType().FirstOrDefault();
            if(accountTypeOptionContext != null)
            {
                command.AccountTypeOption.SetData(accountTypeOptionContext.kind);
            }

            var fundsOptionContext = optionsListContext.optFundsExpr().FirstOrDefault();
            if(fundsOptionContext != null)
            {
                command.FundsOption.SetData(fundsOptionContext.fundsExpr.range);
            }

            var priorityExprOptionContext = optionsListContext.optPriorityExpr().FirstOrDefault();
            if(priorityExprOptionContext != null)
            {
                command.PriorityOption.SetData(priorityExprOptionContext.priority.range);
            }

            return new InterpreterResult<ICommandAction>(command);
        }

        public override InterpreterResult<ICommandAction> VisitNewAccount([NotNull] BudgetCliParser.NewAccountContext context)
        {
            var helpOption = context.optHelp();
            if(helpOption != null)
            {
                var helpCommand = new HelpCommand(context.GetText(), _repositories, Data.Enums.CommandActionKind.AddAccount);
                return new InterpreterResult<ICommandAction>(helpCommand);
            }

            
            var optionsListContext = context.optionsListAccountNew();
            string accountName = context.accountName.Text;
            AddAccountCommand command = new AddAccountCommand(context.GetText(), _repositories, accountName);

            var accountTypeOptionContext = optionsListContext.optAccountType().FirstOrDefault();
            if(accountTypeOptionContext != null)
            {
                command.AccountTypeOption.SetData(accountTypeOptionContext.kind);
            }

            var categoryNameOptionContext = optionsListContext.optCategory().FirstOrDefault();
            if(categoryNameOptionContext != null)
            {
                string categoryName = categoryNameOptionContext.categoryName.Text;
                long categoryId = _repositories.AccountRepository.GetIdByName(categoryName);
                command.CategoryIdOption.SetData(categoryId);
            }

            var descriptionOptionContext = optionsListContext.optDescription().FirstOrDefault();
            if(descriptionOptionContext != null)
            {
                command.DescriptionOption.SetData(descriptionOptionContext.description.Text);
            }

            var fundsOptionContext = optionsListContext.optFunds().FirstOrDefault();
            if(fundsOptionContext != null)
            {
                command.FundsOption.SetData(fundsOptionContext.funds.value);
            }

            var priorityOptionContext = optionsListContext.optPriority().FirstOrDefault();
            if(priorityOptionContext != null)
            {
                command.PriorityOption.SetData(priorityOptionContext.priority.value);
            }

            return new InterpreterResult<ICommandAction>(command);
        }

        public override InterpreterResult<ICommandAction> VisitRemoveAccount([NotNull] BudgetCliParser.RemoveAccountContext context)
        {
            var helpOption = context.optHelp();
            if(helpOption != null)
            {
                var helpCommand = new HelpCommand(context.GetText(), _repositories, Data.Enums.CommandActionKind.RemoveAccount);
                return new InterpreterResult<ICommandAction>(helpCommand);
            }

            DeleteAccountCommand command = new DeleteAccountCommand(context.GetText(), _repositories);
            
            string nameOption = context.accountName.Text;

            command.AccountName.SetData(nameOption);

            if(context.optRecursive() != null)
            {
                command.IsRecursiveOption.SetData(true);
            }

            return new InterpreterResult<ICommandAction>(command);
        }

        public override InterpreterResult<ICommandAction> VisitSetAccount([NotNull] BudgetCliParser.SetAccountContext context)
        {
            throw new NotImplementedException();
        }

        #endregion - Accounts -

        #region - Transactions -

        public override InterpreterResult<ICommandAction> VisitDetailTransaction([NotNull] BudgetCliParser.DetailTransactionContext context)
        {
            throw new NotImplementedException();
        }

        public override InterpreterResult<ICommandAction> VisitListTransactions([NotNull] BudgetCliParser.ListTransactionsContext context)
        {
            throw new NotImplementedException();
        }

        public override InterpreterResult<ICommandAction> VisitNewTransaction([NotNull] BudgetCliParser.NewTransactionContext context)
        {
            throw new NotImplementedException();
        }

        #endregion - Transactions -

        #region - Reporting -

        public override InterpreterResult<ICommandAction> VisitListHistory([NotNull] BudgetCliParser.ListHistoryContext context)
        {
            throw new NotImplementedException();
        }

        #endregion - Reporting -
    }
}