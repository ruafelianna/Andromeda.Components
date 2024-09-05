using Andromeda.Components.Forms.Abstractions;
using DynamicData;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Collections;
using ReactiveUI.Validation.Components.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Formatters;
using ReactiveUI.Validation.Formatters.Abstractions;
using Splat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Andromeda.Components.Forms
{
    public class ValidatableForm :
        Form,
        IValidatableForm,
        INotifyDataErrorInfo,
        IDisposable
    {
        public ValidatableForm(FormState initState) :
            base(initState)
        {
            _disposables = [];
            _mentionedPropertyNames = [];

            _formatter = Locator.Current
                .GetService<IValidationTextFormatter<string>>()
                ?? SingleLineFormatter.Default;

            ValidationContext = new ValidationContext();
            ValidationContext.DisposeWith(_disposables);

            ValidationContext.Validations
                .Connect()
                .ToCollection()
                .Select(cpts => cpts
                    .Select(cpt => cpt
                        .ValidationStatusChange
                        .Select(_ => cpt)
                    )
                    .Merge()
                    .StartWith(ValidationContext)
                )
                .Switch()
                .Subscribe(OnValidationStatusChange)
                .DisposeWith(_disposables);
        }

        public IValidationContext ValidationContext { get; }

        [Reactive]
        public bool HasErrors { get; private set; }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            var validations = InvalidPropertyValidations;

            if (!string.IsNullOrEmpty(propertyName))
            {
                validations = validations
                    .Where(validation => validation
                        .ContainsPropertyName(propertyName)
                    );
            }

            return validations
                .Select(state => _formatter
                    .Format(state.Text ?? ValidationText.None)
                )
                .ToArray();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private readonly CompositeDisposable _disposables;
        private readonly IValidationTextFormatter<string> _formatter;
        private readonly HashSet<string> _mentionedPropertyNames;

        protected void RaiseErrorsChanged(string propertyName = "")
            => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposables.IsDisposed && disposing)
            {
                _disposables.Dispose();
                ValidationContext.Dispose();
                _mentionedPropertyNames.Clear();
            }
        }

        private IEnumerable<IPropertyValidationComponent> InvalidPropertyValidations
            => ValidationContext.Validations.Items
                .OfType<IPropertyValidationComponent>()
                .Where(validation => !validation.IsValid);

        private void OnValidationStatusChange(IValidationComponent component)
        {
            HasErrors = !ValidationContext.GetIsValid();

            if (component is IPropertyValidationComponent propertyValidationComponent)
            {
                foreach (var propertyName in propertyValidationComponent.Properties)
                {
                    RaiseErrorsChanged(propertyName);
                    _mentionedPropertyNames.Add(propertyName);
                }
            }
            else
            {
                foreach (var propertyName in _mentionedPropertyNames)
                {
                    RaiseErrorsChanged(propertyName);
                }
            }
        }
    }
}
