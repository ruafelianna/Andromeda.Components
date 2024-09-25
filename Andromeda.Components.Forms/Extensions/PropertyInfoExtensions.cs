using Andromeda.Components.Forms.Abstractions;
using Andromeda.Components.Forms.DataTypes;
using Andromeda.Components.Forms.FormFields;
using Andromeda.Components.Forms.FormFields.DataTypes;
using System;
using System.Reflection;

namespace Andromeda.Components.Forms.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsFormField(this PropertyInfo pi)
            => pi.IsDefined(typeof(FormFieldDataTypeAttribute), true);

        public static bool IsText(this PropertyInfo pi)
            => pi.IsDefined(typeof(FormField_TextAttribute), true);

        public static bool IsBool(this PropertyInfo pi)
            => pi.IsDefined(typeof(FormField_BoolAttribute), true);

        public static bool IsChoice(this PropertyInfo pi)
            => pi.IsDefined(typeof(FormField_ChoiceAttribute), true);

        public static bool IsNumber(this PropertyInfo pi)
            => pi.IsDefined(typeof(FormField_NumberAttribute), true);

        public static bool CanRead(
            this PropertyInfo pi,
            FormState state
        ) => ((pi.GetCustomAttribute<FormField_CanReadAttribute>()?
                .State & state) ?? AllState) == state;

        public static bool CanWrite(
            this PropertyInfo pi,
            FormState state
        ) => ((pi.GetCustomAttribute<FormField_CanWriteAttribute>()?
                .State & state) ?? AllState) == state;

        public static int Order(this PropertyInfo pi)
            => pi.GetCustomAttribute<FormField_OrderAttribute>()?
                .Order ?? int.MaxValue;

        public static string? Label(this PropertyInfo pi)
            => pi.GetCustomAttribute<FormField_LabelAttribute>()?
                .Label;

        public static string? Hint(this PropertyInfo pi)
            => pi.GetCustomAttribute<FormField_HintAttribute>()?
                .Hint;

        public static FormField_ChoiceAttribute ChoiceInfo(this PropertyInfo pi)
            => pi.GetCustomAttribute<FormField_ChoiceAttribute>()!;

        public static IFormFieldDataType DataType(this PropertyInfo pi)
        {
            if (pi.IsText())
            {
                return new TextDataType();
            }
            else if (pi.IsBool())
            {
                return new BoolDataType();
            }
            else if (pi.IsChoice())
            {
                var choice = pi.ChoiceInfo();

                return new ChoiceDataType(
                    choice.OptionsPropertyName,
                    choice.UpdateCommandName
                );
            }
            else if (pi.IsNumber())
            {
                return new NumberDataType();
            }
            throw new Exception("Unknown form field data type.");
        }

        private const FormState AllState
            = FormState.Create | FormState.Edit | FormState.View;
    }
}
