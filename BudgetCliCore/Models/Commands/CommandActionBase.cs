using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCliData.Enums;
using BudgetCliData.Models;
using BudgetCliData.Repositories;
using BudgetCliUtil.Attributes;

namespace BudgetCliCore.Models.Commands
{
    public abstract class CommandActionBase<T> : ICommandAction<T>, IDataModel<CommandActionDto>
    {
        [HelpInfo(Visible: false)]
        protected RepositoryBag Repositories { get; }

        
        [HelpInfo("Id", "A unique ID used to reference a command")]
        public long? Id { get; set; }

        [HelpInfo(Visible: false)]
        public virtual string RawText { get; }

        [HelpInfo("Description", "A brief description of the command/action")]
        public abstract string ActionText { get; }

        [HelpInfo("Execution Status", "A flag that notes if the command has been executed at the current time. True = executed; False = not executed")]
        public virtual bool IsExecuted { get; private set; }

        [HelpInfo("Timestamp", "The date/time the command was, or will be, executed")]
        public DateTime Timestamp { get; }

        [HelpInfo(Visible: false)]
        public abstract CommandActionKind CommandActionKind { get; }

        /// <summary>
        /// New Command constructor
        /// </summary>
        public CommandActionBase(string rawText, RepositoryBag repositories)
        {
            RawText = rawText;
            Timestamp = DateTime.Now;
            Repositories = repositories;
            IsExecuted = false;
        }
        
        /// <summary>
        /// From Dto constructor
        /// </summary>
        public CommandActionBase(long id, string rawText, bool isExecuted, DateTime timestamp, RepositoryBag repositories)
        {
            Id = id;
            RawText = rawText;
            IsExecuted = false;
            Timestamp = DateTime.Now;
            Repositories = repositories;
        }

        public bool Execute(out T result)
        {
            if(TryDoAction(out result))
            {
                IsExecuted = true;
                Repositories.CommandActionRepository.Upsert(ToDto());

                var parameterDtos = GetParameterDtos();
                foreach(var paramDto in parameterDtos)
                {
                    //Parameters are immutable, so only upsert if they have no id (and thus have not been inserted yet)
                    if(!paramDto.Id.HasValue)
                    {
                        Repositories.CommandActionParameterRepository.Upsert(paramDto);
                    }
                }
                return true;
            }

            return false;
        }

        public bool Undo(out T result)
        {
            if(TryUndoAction(out result))
            {
                IsExecuted = false;
                Repositories.CommandActionRepository.Upsert(ToDto());
                //Don't need to touch the parameters. They are immutable
                
                return true;
            }
            return false;
        }

        protected abstract bool TryDoAction(out T result);

        protected abstract bool TryUndoAction(out T result);

        protected abstract IEnumerable<CommandActionParameterDto> GetParameterDtos();

        public CommandActionDto ToDto()
        {
            CommandActionDto dto = new CommandActionDto();
            dto.Id = this.Id;
            dto.Timestamp = this.Timestamp;
            dto.CommandText = ActionText;
            dto.CommandActionKind = this.CommandActionKind;
            return dto;
        }
    }
}