namespace BudgetCli.Data.Enums
{
    public enum CommandActionKind
    {
        Version = -5,
        Exit = -4,
        Clear = -3,
        Help = -2,
        NonPersisted = -1,
        
        AddAccount = 0,
        RemoveAccount = 1,
        UpdateAccount = 2,
        ListAccount = 3,
        DetailAccount = 4,

        AddTransaction = 100,
        ListTransaction = 101,
        DetailTransaction = 102,

        ListHistory = 200,
    }
}