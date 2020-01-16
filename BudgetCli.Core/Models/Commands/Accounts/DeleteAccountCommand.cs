using System;
using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Logging;

namespace BudgetCli.Core.Models.Commands.Accounts
{
    public class DeleteAccountCommand : CommandActionBase
    {
        public override string ActionText 
        {
            get
            {
                return $"Deleted account \"{AccountName.GetValue(String.Empty)}\"";
            }
        }

        public override CommandActionKind CommandActionKind 
        {
            get { return CommandActionKind.RemoveAccount; }
        }

        //Options
        public StringCommandOption AccountName { get; set; }
        public BoolCommandOption IsRecursiveOption { get; set; }

        public DeleteAccountCommand(string rawText, RepositoryBag repositories) : base(rawText, repositories)
        {
            AccountName = new StringCommandOption(CommandOptionKind.Name);
            IsRecursiveOption = new BoolCommandOption(CommandOptionKind.Recursive);
        }

        protected override bool TryDoAction(ILog log, IEnumerable<ICommandActionListener> listeners = null)
        {
            long accountId = Repositories.AccountRepository.GetIdByName(AccountName.GetValue(String.Empty));

            Console.WriteLine("Found account with id " + accountId);
            List<Account> deletedAccounts = new List<Account>();
            bool isSuccessful = DeleteAccount(accountId, IsRecursiveOption.GetValue(false), log, deletedAccounts);

            DeleteCommandResult<Account> result = new DeleteCommandResult<Account>(this, isSuccessful, deletedAccounts);
            
            TransmitResult(result, listeners);
            return isSuccessful;
        }

        private bool DeleteAccount(long id, bool isRecursive, ILog log, List<Account> deletedAccounts)
        {
            //Populate list
            AccountDto dto = Repositories.AccountRepository.GetById(id);
            Account deletedAccount = DtoToModelTranslator.FromDto(dto, Repositories);
            deletedAccounts.Add(deletedAccount);

            //Delete the account
            bool successful = Repositories.AccountStateRepository.CloseAccount(id);
            
            //Delete child accounts (if recursive)
            if(isRecursive)
            {
                IEnumerable<long> childAccountIds = Repositories.AccountRepository.GetChildAccountIds(id);
                foreach(var child in childAccountIds)
                {
                    successful &= DeleteAccount(id, isRecursive, log, deletedAccounts);
                }    
            }

            return successful;
        }
        

        private void TransmitResult(DeleteCommandResult<Account> result, IEnumerable<ICommandActionListener> listeners = null)
        {
            if(listeners != null && result != null)
            {
                foreach(var listener in listeners)
                {
                    listener.OnCommand(result);
                }
            }
        }
    }
}