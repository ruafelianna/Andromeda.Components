using Andromeda.Components.Avalonia.Abstractions;
using Andromeda.Components.Avalonia.ViewModels;
using Andromeda.Components.Forms.Abstractions;
using Andromeda.Components.Forms.DataTypes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI.Validation.Contexts;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Andromeda.Components.Avalonia.Helpers
{
    internal static class FormFieldsBuildHelper
    {
        public static TextBlock CreateLabel(
            this IFormFieldInfo fi
        )
        {
            var label = new TextBlock
            {
                Text = fi.Label,
                [!DataValidationErrors.ErrorsProperty]
                    = new Binding(nameof(ValidationContext)),
                Classes = {
                    "form-label",
                },
            };
            label.Loaded += (sender, args) => {
                var vm = label.GetParentDataContext()?.Form;
            };
            
            return label;
        }

        public static Control CreateControl(
            this IFormFieldInfo fi
        )
        {
            if (!CreateControlFuncs.TryGetValue(
                fi.DataType.Handle,
                out var createControl
            ))
            {
                return fi.RenderEmpty();
            }

            var control = createControl(fi);

            control.Classes.Add("form-control");

            control.Events().Loaded
                .Subscribe(x =>
                {
                    var vm = control.GetParentDataContext()?.Form;

                    if (BindControlFuncs.TryGetValue(
                        fi.DataType.Handle,
                        out var bindControl
                    ))
                    {
                        bindControl(control, fi, vm);
                    }
                });

            return control;
        }

        #region Render

        private static TextBox RenderTextBox(
            this IFormFieldInfo fi
        ) => new();

        private static CheckBox RenderCheckBox(
            this IFormFieldInfo fi
        ) => new();

        private static ComboBox RenderComboBox(
            this IFormFieldInfo fi
        ) => new();

        private static TextBlock RenderEmpty(
            this IFormFieldInfo fi
        ) => new() { Text = fi.ToString() };

        #endregion

        #region Bind

        private static void BindTextBox(
            this Control control,
            IFormFieldInfo fi,
            object? dataContext = null
        ) => control.MakeBinding(
            fi.PropertyName,
            TextBox.TextProperty,
            dataContext
        );

        private static void BindCheckBox(
            this Control control,
            IFormFieldInfo fi,
            object? dataContext = null
        ) => control.MakeBinding(
            fi.PropertyName,
            ToggleButton.IsCheckedProperty,
            dataContext
        );

        public static void BindComboBox(
            this Control control,
            IFormFieldInfo fi,
            object? dataContext = null
        )
        {
            control.MakeBinding(
                fi.PropertyName,
                SelectingItemsControl.SelectedItemProperty,
                dataContext
            );

            control.MakeBinding(
                ((ChoiceDataType)fi.DataType)!.OptionsPropertyName,
                ItemsControl.ItemsSourceProperty,
                dataContext
            );
        }

        #endregion

        #region Utils

        private static readonly Dictionary<RuntimeTypeHandle,
            Func<IFormFieldInfo, Control>> CreateControlFuncs = new()
            {
                { typeof(TextDataType).TypeHandle, RenderTextBox },
                { typeof(BoolDataType).TypeHandle, RenderCheckBox },
                { typeof(ChoiceDataType).TypeHandle, RenderComboBox },
                { typeof(NumberDataType).TypeHandle, RenderTextBox },
            };

        private static readonly Dictionary<RuntimeTypeHandle,
            Action<Control, IFormFieldInfo, object?>> BindControlFuncs = new()
            {
                { typeof(TextDataType).TypeHandle, BindTextBox },
                { typeof(BoolDataType).TypeHandle, BindCheckBox },
                { typeof(ChoiceDataType).TypeHandle, BindComboBox },
                { typeof(NumberDataType).TypeHandle, BindTextBox },
            };

        private static TreeDataGridFormViewModel? GetParentDataContext(
            this Control control
        )
        {
            var parent = control.Parent;

            while (parent is not null && parent is not IFormControl)
            {
                parent = parent.Parent;
            }

            return parent?.DataContext as TreeDataGridFormViewModel;
        }

        private static void MakeBinding(
            this Control control,
            string propertyName,
            AvaloniaProperty controlProperty,
            object? dataContext = null,
            IValueConverter? converter = null
        )
        {
            var binding = new Binding(propertyName);

            if (dataContext is not null)
            {
                binding.Source = dataContext;
            }

            if (converter is not null)
            {
                binding.Converter = converter;
            }

            control.Bind(controlProperty, binding);
        }

        #endregion
    }
}
