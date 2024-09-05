using Andromeda.Components.Avalonia.Helpers;
using Andromeda.Components.Forms.Abstractions;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Andromeda.Components.Avalonia.Resources
{
    internal class FormField_Label_TemplateSelector : IDataTemplate
    {
        public Control? Build(object? param)
            => (param as IFormFieldInfo)!.CreateLabel();

        public bool Match(object? data)
            => data is IFormFieldInfo;
    }
}
