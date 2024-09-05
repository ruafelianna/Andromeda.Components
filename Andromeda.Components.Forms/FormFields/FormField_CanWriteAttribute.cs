using Andromeda.Components.Forms.Abstractions;

namespace Andromeda.Components.Forms.FormFields
{
    public sealed class FormField_CanWriteAttribute :
        BaseFormFieldAttribute
    {
        public FormField_CanWriteAttribute(FormState state)
        {
            State = state;
        }

        public FormState State { get; }
    }
}
