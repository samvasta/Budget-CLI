using System;
using BudgetCliCore.Exceptions;
using BudgetCliCore.Models.Commands;
using BudgetCliData.Enums;
using BudgetCliData.Models;
using BudgetCliData.Repositories;

namespace BudgetCliCore.Models
{
    public static class DtoToModelTranslator
    {

        public static Account FromDto(AccountDto dto, RepositoryBag repositories)
        {
            throw new NotImplementedException();
        }

        public static CommandActionBase FromDto(CommandActionDto dto, RepositoryBag repositories)
        {
            if(dto.CommandActionKind == BudgetCliData.Enums.CommandActionKind.AddAccount)
            {
                return FromAddAccountDto(dto, repositories);
            }
            else if(dto.CommandActionKind == BudgetCliData.Enums.CommandActionKind.RemoveAccount)
            {
                return FromRemoveAccountDto(dto, repositories);
            }
            else if(dto.CommandActionKind == BudgetCliData.Enums.CommandActionKind.UpdateAccount)
            {
                return FromUpdateAccountDto(dto, repositories);
            }
            else if(dto.CommandActionKind == BudgetCliData.Enums.CommandActionKind.AddTransaction)
            {
                return FromAddTransactionDto(dto, repositories);
            }
            else if(dto.CommandActionKind == BudgetCliData.Enums.CommandActionKind.RemoveTransaction)
            {
                return FromRemoveTransactionDto(dto, repositories);
            }
            else if(dto.CommandActionKind == BudgetCliData.Enums.CommandActionKind.UpdateTransaction)
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