using Andromeda.Components.Avalonia.Abstractions;
using Andromeda.Components.Avalonia.ViewModels;
using Avalonia.ReactiveUI;

namespace Andromeda.Components.Avalonia.Controls;

public partial class TreeDataGridForm :
    ReactiveUserControl<TreeDataGridFormViewModel>,
    IFormControl
{
    public TreeDataGridForm()
    {
        InitializeComponent();
    }
}
