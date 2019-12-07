using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace BudgetCliUtil.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum)]
    public class HelpInfoAttribute : Attribute
    {
        private string _displayName;
        private string _description;
        private bool _visible;

        public HelpInfoAttribute([CallerMemberName] string DisplayName = "", string Description = "", bool Visible = true)
        {
            _displayName = DisplayName;
            _description = Description;
            _visible = Visible;
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public string Description
        {
            get { return _description; }
        }

        public bool Visible
        {
            get { return _visible; }
        }
    }
}