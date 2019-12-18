namespace BudgetCli.Data.Enums
{
    public enum CommandActionKind
    {
        NonPersisted = -1,
        
        AddAccount = 0,
        RemoveAccount = 1,
        UpdateAccount = 2,

        AddTransaction = 100,
        RemoveTransaction = 101,
        UpdateTransaction = 102
    }
}