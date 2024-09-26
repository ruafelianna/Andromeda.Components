using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace Andromeda.Components.Avalonia.Controls
{
    public class CommandControl : ContentControl
    {
        public static readonly StyledProperty<ICommand?> CommandProperty =
            AvaloniaProperty.Register<Button, ICommand?>(
                nameof(Command)
            );

        public ICommand? Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
    }
}
