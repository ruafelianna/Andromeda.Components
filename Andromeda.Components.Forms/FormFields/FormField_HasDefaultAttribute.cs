using Andromeda.CSharp.Enums;

namespace Andromeda.Components.Forms.FormFields
{
    public class FormField_HasDefaultAttribute :
        BaseFormFieldAttribute
    {
        public AccessModifier GetAccess { get; set; }

        public AccessModifier SetAccess { get; set; }

        public bool IsInit { get; set; }

        public bool IsReactive { get; set; }
    }
}
