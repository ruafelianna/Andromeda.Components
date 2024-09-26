using Andromeda.Components.Forms.Abstractions;
using Andromeda.Components.Forms.Assets;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Andromeda.Components.Forms.Extensions
{
    public static class ValidationExtensions
    {
        public static void EmptyRule<TViewModel>(
            this TViewModel vm,
            Expression<Func<TViewModel, string?>> property,
            string? message = null
        ) where TViewModel : IValidatableForm
            => vm.ValidationRule(
                property,
                vm.WhenAnyValue(property)
                    .Select(x => !string.IsNullOrWhiteSpace(x)),
                message ?? ValidationStrings.CannotBeEmpty
            );

        public static void EqualsRule<TViewModel, TProperty>(
            this TViewModel vm,
            Expression<Func<TViewModel, TProperty?>> property,
            Func<TProperty> getValue,
            Func<TProperty?, string>? format = null,
            string? message = null
        )
            where TViewModel : IValidatableForm
            where TProperty : IEquatable<TProperty>
        {
            var value = getValue();

            vm.ValidationRule(
                property,
                vm.WhenAnyValue(property)
                    .Select(x =>
                        (x is null && value is null)
                        || (x?.Equals(value) ?? false)
                    ),
                message
                ?? string.Format(
                    ValidationStrings.ShouldBeEqual,
                    format?.Invoke(value) ?? value.ToString()
                )
            );
        }

        public static void NotEqualsRule<TViewModel, TProperty>(
            this TViewModel vm,
            Expression<Func<TViewModel, TProperty?>> property,
            Func<TProperty> getValue,
            Func<TProperty?, string>? format = null,
            string? message = null
        )
            where TViewModel : IValidatableForm
            where TProperty : IEquatable<TProperty>
        {
            var value = getValue();

            vm.ValidationRule(
                property,
                vm.WhenAnyValue(property)
                    .Select(x =>
                        !(x is null && value is null)
                        && !(x?.Equals(value) ?? false)
                    ),
                message
                ?? string.Format(
                    ValidationStrings.ShouldNotBeEqual,
                    format?.Invoke(value) ?? value.ToString()
                )
            );
        }

        public static void ParseRule<TViewModel, TParsable>(
            this TViewModel vm,
            Expression<Func<TViewModel, string?>> property,
            IFormatProvider? provider = null,
            string? message = null
        )
            where TViewModel : IValidatableForm
            where TParsable : IParsable<TParsable>
            => vm.ValidationRule(
                property,
                vm.WhenAnyValue(property)
                    .Select(x => string.IsNullOrWhiteSpace(x)
                        || TParsable.TryParse(
                            x,
                            provider ?? CultureInfo.CurrentCulture,
                            out _
                        )
                    ),
                message ?? ValidationStrings.CannotParseAs
            );

        public static void LinkParsable<TViewModel, TParsable>(
            this TViewModel vm,
            Expression<Func<TViewModel, TParsable?>> property,
            Expression<Func<TViewModel, string?>> parsable,
            IFormatProvider? provider = null
        )
            where TViewModel : ReactiveObject, IValidatableForm
            where TParsable : struct, IParsable<TParsable>
            => vm.WhenAnyValue(parsable)
                .Select(x => TParsable
                    .TryParse(
                        x?.Trim(),
                        provider ?? CultureInfo.CurrentCulture,
                        out var result
                    )
                        ? (TParsable?)result
                        : null
                )
                .ToPropertyEx(vm, property);
    }
}
