namespace Andromeda.Components.Forms.FormFields
{
    public sealed class FormField_HintAttribute :
        BaseFormFieldAttribute
    {
        public FormField_HintAttribute(string hint)
        {
            Hint = hint;
        }

        public string Hint { get; }
    }
}
