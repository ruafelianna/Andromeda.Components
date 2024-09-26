using Andromeda.Components.Avalonia.Resources;
using Andromeda.Components.Forms.Abstractions;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using ReactiveUI;

namespace Andromeda.Components.Avalonia.ViewModels
{
    public class TreeDataGridFormViewModel : ReactiveObject
    {
        public TreeDataGridFormViewModel(IForm form)
        {
            Form = form;

            Fields = new(form.FormFields)
            {
                Columns = {
                    new TemplateColumn<IFormFieldInfo>("",
                        new FormField_Label_TemplateSelector()),
                    new TemplateColumn<IFormFieldInfo>("",
                        new FormField_Control_TemplateSelector()),
                },
            };
        }

        public IForm Form { get; }

        public FlatTreeDataGridSource<IFormFieldInfo> Fields { get; }
    }
}
