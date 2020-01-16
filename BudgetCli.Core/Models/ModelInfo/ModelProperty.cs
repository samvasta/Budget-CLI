using System;
namespace BudgetCli.Core.Models.ModelInfo
{
    public class ModelProperty
    {
        public string DisplayName { get; }
        public string Description { get; }
        public bool IsVisibleInList { get; }
        public bool IsVisibleInDetails { get; }

        public ModelProperty(string displayName, string description, bool isVisibleInList = false, bool isVisibleInDetails = true)
        {
            DisplayName = displayName;
            Description = description;
            IsVisibleInList = isVisibleInList;
            IsVisibleInDetails = isVisibleInDetails;
        }

        public override bool Equals(object obj)
        {
            if(obj is ModelProperty modelProp)
            {
                return this.DisplayName.Equals(modelProp.DisplayName) &&
                       this.Description.Equals(modelProp.Description) &&
                       this.IsVisibleInDetails.Equals(modelProp.IsVisibleInDetails) &&
                       this.IsVisibleInList.Equals(modelProp.IsVisibleInDetails);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DisplayName, Description, IsVisibleInDetails, IsVisibleInList);
        }
    }
}