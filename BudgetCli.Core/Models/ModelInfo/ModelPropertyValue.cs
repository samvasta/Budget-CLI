namespace BudgetCli.Core.Models.ModelInfo
{
    public class ModelPropertyValue
    {
        public virtual ModelProperty Property { get; }
        public virtual bool IsDefaultValue { get; }
        public virtual object Value { get; }

        public ModelPropertyValue(ModelProperty property)
        {
            Property = property;
            IsDefaultValue = true;
        }
        public ModelPropertyValue(ModelProperty property, object value)
        {
            Property = property;
            IsDefaultValue = false;
            Value = value;
        }
    }

    public class ModelPropertyValue<T> : ModelPropertyValue
    {
        public virtual new T Value { get; }

        public ModelPropertyValue(ModelProperty property)
            : base(property)
        {
        }
        public ModelPropertyValue(ModelProperty property, T value)
            : base(property, value)
        {
        }
    }
}