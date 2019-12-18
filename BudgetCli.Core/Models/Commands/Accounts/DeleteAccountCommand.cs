using System;
using System.Collections.Generic;
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
            return DeleteAccount(accountId, IsRecursiveOption.GetValue(false), log);
        }

        private bool DeleteAccount(long id, bool isRecursive, ILog log)
        {
            IEnumerable<long> childAccountIds = Repositories.AccountRepository.GetChildAccountIds(id);

            bool successful = Repositories.AccountStateRepository.CloseAccount(id);
            
            if(isRecursive)
            {
                foreach(var child in childAccountIds)
                {
                    successful &= DeleteAccount(id, isRecursive, log);
                }    
            }

            return successful;
        }
    }
}