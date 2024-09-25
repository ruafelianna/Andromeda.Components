using static Andromeda.CSharp.Consts.Postfixes;

namespace Andromeda.Components.Forms.SourceGenerators
{
    internal static class InternalConsts
    {
        public const string NS_Fody
            = "ReactiveUI.Fody.Helpers";

        // --------------------------------------------

        public const string NS_Root = nameof(Andromeda);

        public const string NS_Forms
            = $"{NS_Root}.Components.Forms";

        public const string NS_Forms_Abstractions
            = $"{NS_Forms}.Abstractions";

        public const string NS_Forms_FormFields
            = $"{NS_Forms}.FormFields";

        // --------------------------------------------

        public const string I_IForm = "IForm";

        // --------------------------------------------

        public const string A_BaseFormFieldFull
            = $"BaseFormField{POST_Attribute}";

        // --------------------------------------------

        public const string A_HasDefault = "FormField_HasDefault";

        public const string A_HasDefaultFull
            = $"{A_HasDefault}{POST_Attribute}";

        public const string P_HasDefault_GetAccess = "GetAccess";

        public const string P_HasDefault_SetAccess = "SetAccess";

        public const string P_HasDefault_IsInit = "IsInit";

        public const string P_HasDefault_IsReactive = "IsReactive";

        // --------------------------------------------

        public const string NewLine = "\r\n";

        public const string Reactive = "[Reactive]";

        public const string Unknown = nameof(Unknown);

        public const string PropTab = "        ";
    }
}
