using Andromeda.Components.Forms.Abstractions;

namespace Andromeda.Components.Forms.FormFields
{
    public sealed class FormField_CanReadAttribute :
        BaseFormFieldAttribute
    {
        public FormField_CanReadAttribute(FormState state)
        {
            State = state;
        }

        public FormState State { get; }
    }
}
