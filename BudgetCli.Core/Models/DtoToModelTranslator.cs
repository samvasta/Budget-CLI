using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Core.Exceptions;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Commands.Accounts;
using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models
{
    public static class DtoToModelTranslator
    {

        public static Account FromDto(AccountDto dto, RepositoryBag repositories)
        {
            return new Account(dto.Id.Value, dto.Name, dto.CategoryId, dto.Priority, dto.AccountKind, dto.Description, repositories);
        }

        public static AccountState FromDto(AccountStateDto dto, RepositoryBag repositories)
        {
            return new AccountState(dto.Id.Value, dto.AccountId, new Money(dto.Funds, true), dto.Timestamp, dto.IsClosed);
        }

        public static Transaction FromDto(TransactionDto dto, RepositoryBag repositories)
        {
            return new Transaction(dto.Id.Value, dto.Timestamp, dto.SourceAccountId, dto.DestinationAccountId, dto.TransferAmount, dto.Memo, repositories);
        }

    }
}