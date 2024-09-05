namespace Andromeda.Components.Forms.FormFields.DataTypes
{
    public sealed class FormField_ChoiceAttribute :
        FormFieldDataTypeAttribute
    {
        public FormField_ChoiceAttribute(
            string optionsPropertyName,
            string updateCommandName
        )
        {
            OptionsPropertyName = optionsPropertyName;
            UpdateCommandName = updateCommandName;
        }

        public string OptionsPropertyName { get; }

        public string UpdateCommandName { get; }
    }
}
