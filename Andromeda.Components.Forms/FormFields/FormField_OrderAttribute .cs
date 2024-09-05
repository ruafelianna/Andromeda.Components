namespace Andromeda.Components.Forms.FormFields
{
    public sealed class FormField_OrderAttribute :
        BaseFormFieldAttribute
    {
        public FormField_OrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; }
    }
}
