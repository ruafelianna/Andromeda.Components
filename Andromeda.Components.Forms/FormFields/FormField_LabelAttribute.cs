namespace Andromeda.Components.Forms.FormFields
{
    public sealed class FormField_LabelAttribute :
        BaseFormFieldAttribute
    {
        public FormField_LabelAttribute(string label)
        {
            Label = label;
        }

        public string Label { get; }
    }
}
