using Andromeda.Components.Avalonia.Resources;
using Andromeda.Components.Forms;
using Andromeda.Components.Forms.Abstractions;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using System;
using System.Threading.Tasks;

namespace Andromeda.Components.Avalonia.ViewModels
{
    public abstract class TreeDataGridFormViewModel :
        SaveableForm<TreeDataGridFormViewModel>
    {
        public TreeDataGridFormViewModel(
            FormState initState,
            Func<TreeDataGridFormViewModel, Task<bool>> save,
            IObservable<bool>? canSaveExt = null
        ) : base(initState, save, canSaveExt)
        {
            Fields = new(FormFields)
            {
                Columns = {
                    new TemplateColumn<IFormFieldInfo>("",
                        new FormField_Label_TemplateSelector()),
                    new TemplateColumn<IFormFieldInfo>("",
                        new FormField_Control_TemplateSelector()),
                },
            };
        }

        public FlatTreeDataGridSource<IFormFieldInfo> Fields { get; }
    }
}
