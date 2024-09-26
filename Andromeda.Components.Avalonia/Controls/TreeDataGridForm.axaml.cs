using Andromeda.Components.Avalonia.Abstractions;
using Andromeda.Components.Avalonia.ViewModels;
using Avalonia;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Andromeda.Components.Avalonia.Controls;

public partial class TreeDataGridForm :
    ReactiveUserControl<TreeDataGridFormViewModel>,
    IFormControl
{
    public TreeDataGridForm()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<IReactiveObject?> SaveContentProperty =
        AvaloniaProperty.Register<TreeDataGridForm, IReactiveObject?>(
            nameof(ResetContent)
        );

    public IReactiveObject? SaveContent
    {
        get => GetValue(SaveContentProperty);
        set => SetValue(SaveContentProperty, value);
    }

    public static readonly StyledProperty<IReactiveObject?> ResetContentProperty =
        AvaloniaProperty.Register<TreeDataGridForm, IReactiveObject?>(
            nameof(ResetContent)
        );

    public IReactiveObject? ResetContent
    {
        get => GetValue(ResetContentProperty);
        set => SetValue(ResetContentProperty, value);
    }
}
