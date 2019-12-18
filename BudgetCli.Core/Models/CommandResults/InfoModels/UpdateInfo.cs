namespace BudgetCli.Core.Models.CommandResults.InfoModels
{
    /// <summary>
    /// Holds information about a change in a field's value. Meant for display purposes only
    /// </summary>
    public class UpdateInfo
    {
        /// <summary>
        /// Name of field that changed
        /// </summary>
        public string FieldName { get; }
        
        /// <summary>
        /// Value of field before change, as string
        /// </summary>
        public string OldValueStr { get; }

        /// <summary>
        ///Value of field after change, as string
        /// </summary>
        public string NewValueStr { get; }

        public UpdateInfo(string fieldName, string oldValueStr, string newValueStr)
        {
            FieldName = fieldName;
            OldValueStr = oldValueStr;
            NewValueStr = newValueStr;
        }
    }
}