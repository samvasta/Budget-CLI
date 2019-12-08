using System;
using BudgetCli.Core.Exceptions;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;

namespace BudgetCli.Core.Models
{
    public static class DtoToModelTranslator
    {

        public static Account FromDto(AccountDto dto, RepositoryBag repositories)
        {
            throw new NotImplementedException();
        }

        public static CommandActionBase FromDto(CommandActionDto dto, RepositoryBag repositories)
        {
            if(dto.CommandActionKind == BudgetCli.Data.Enums.CommandActionKind.AddAccount)
            {
                return FromAddAccountDto(dto, repositories);
            }
            else if(dto.CommandActionKind == BudgetCli.Data.Enums.CommandActionKind.RemoveAccount)
            {
                return FromRemoveAccountDto(dto, repositories);
            }
            else if(dto.CommandActionKind == BudgetCli.Data.Enums.CommandActionKind.UpdateAccount)
            {
                return FromUpdateAccountDto(dto, repositories);
            }
            else if(dto.CommandActionKind == BudgetCli.Data.Enums.CommandActionKind.AddTransaction)
            {
                return FromAddTransactionDto(dto, repositories);
            }
            else if(dto.CommandActionKind == BudgetCli.Data.Enums.CommandActionKind.RemoveTransaction)
            {
                return FromRemoveTransactionDto(dto, repositories);
            }
            else if(dto.CommandActionKind == BudgetCli.Data.Enums.CommandActionKind.UpdateTransaction)
            {
                return FromUpdateTransactionDto(dto, repositories);
            }

            throw new UnknownEnumException<CommandActionKind>(dto.CommandActionKind, "Failed to load Command");
        }

        public static Transaction FromDto(TransactionDto dto, RepositoryBag repositories)
        {
            return new Transaction(dto.Id.Value, dto.Timestamp, dto.SourceAccountId, dto.DestinationAccountId, dto.TransferAmount, dto.Memo, repositories);
        }


        #region - Commands -
        
        private static CommandActionBase FromAddAccountDto(CommandActionDto dto, RepositoryBag repositories)
        {
            //TODO:
            throw new NotImplementedException();
        }

        private static CommandActionBase FromRemoveAccountDto(CommandActionDto dto, RepositoryBag repositories)
        {
            //TODO:
            throw new NotImplementedException();
        }

        private static CommandActionBase FromUpdateAccountDto(CommandActionDto dto, RepositoryBag repositories)
        {
            //TODO:
            throw new NotImplementedException();
        }

        private static CommandActionBase FromAddTransactionDto(CommandActionDto dto, RepositoryBag repositories)
        {
            //TODO:
            throw new NotImplementedException();
        }

        private static CommandActionBase FromRemoveTransactionDto(CommandActionDto dto, RepositoryBag repositories)
        {
            //TODO:
            throw new NotImplementedException();
        }

        private static CommandActionBase FromUpdateTransactionDto(CommandActionDto dto, RepositoryBag repositories)
        {
            //TODO:
            throw new NotImplementedException();
        }

        #endregion - Commands -
    }
}