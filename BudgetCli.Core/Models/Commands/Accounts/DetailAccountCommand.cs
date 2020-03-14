using System.Collections.Generic;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.CommandResults.InfoModels;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Logging;

namespace BudgetCli.Core.Models.Commands.Accounts
{
    public class DetailAccountCommand : CommandActionBase
    {
        private const string EMPTY_OPTION = "<EMPTY>";


        public override string ActionText
        {
            get
            {
                return "Listed accounts";
            }
        }

        public override CommandActionKind CommandActionKind { get { return CommandActionKind.ListAccount; } }

        //Options
        public StringCommandOption NameOption { get; set; }
        public DateCommandOption DateOption { get; set; }

        public DetailAccountCommand(string rawText, RepositoryBag repositories) : base(rawText, repositories)
        {
            NameOption = new StringCommandOption(CommandOptionKind.Name);
            DateOption = new DateCommandOption(CommandOptionKind.Date);
        }

        protected override bool TryDoAction(ILog log, IEnumerable<ICommandActionListener> listeners = null)
        {
            Account account = GetAccount();

            FilterCriteria criteria = GetFilterCriteria();

            ReadDetailsCommandResult<Account> result = new ReadDetailsCommandResult<Account>(this, true, account, criteria);
            
            TransmitResult(result, listeners);
            return true;
        }

        private Account GetAccount()
        {
            AccountDto dto = Repositories.AccountRepository.GetByName(NameOption.GetValue(null));

            return DtoToModelTranslator.FromDto(dto, Repositories);
        }

        public FilterCriteria GetFilterCriteria()
        {
            FilterCriteria criteria = new FilterCriteria();

            //Name
            if(NameOption.IsDataValid)
            {
                criteria.AddField(Account.PROP_NAME.DisplayName, $"contains \"{NameOption.GetValue(EMPTY_OPTION)}\"");
            }

            //Priority
            if(DateOption.IsDataValid)
            {
                criteria.AddField("Date", $"= {DateOption.GetValue(EMPTY_OPTION)}");
            }

            return criteria;
        }

        private void TransmitResult(ReadDetailsCommandResult<Account> result, IEnumerable<ICommandActionListener> listeners = null)
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