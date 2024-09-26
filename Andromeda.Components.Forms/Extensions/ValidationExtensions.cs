using Andromeda.Components.Forms.Abstractions;
using Andromeda.Components.Forms.Assets;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
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
            TProperty? value,
            Func<TProperty?, string>? format = null,
            string? message = null
        )
            where TViewModel : IValidatableForm
            where TProperty : IEquatable<TProperty>
            => vm.ValidationRule(
                property,
                vm.WhenAnyValue(property)
                    .Select(x =>
                        (x is null && value is null)
                        || (x?.Equals(value) ?? false)
                    ),
                message
                ?? string.Format(
                    ValidationStrings.ShouldBeEqual,
                    format?.Invoke(value) ?? value?.ToString()
                )
            );

        public static void NotEqualsRule<TViewModel, TProperty>(
            this TViewModel vm,
            Expression<Func<TViewModel, TProperty?>> property,
            TProperty? value,
            Func<TProperty?, string>? format = null,
            string? message = null
        )
            where TViewModel : IValidatableForm
            where TProperty : IEquatable<TProperty>
            => vm.ValidationRule(
                property,
                vm.WhenAnyValue(property)
                    .Select(x =>
                        !(x is null && value is null)
                        && !(x?.Equals(value) ?? false)
                    ),
                message
                ?? string.Format(
                    ValidationStrings.ShouldNotBeEqual,
                    format?.Invoke(value) ?? value?.ToString()
                )
            );

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

        public static void GreaterRule<TViewModel, TComparable>(
            this TViewModel vm,
            Expression<Func<TViewModel, string?>> property,
            Expression<Func<TViewModel, TComparable?>> comparable,
            TComparable value,
            Func<TComparable?, string>? format = null,
            string? message = null
        )
            where TViewModel : IValidatableForm
            where TComparable : struct, IComparisonOperators<TComparable, TComparable, bool>
            => vm.ValidationRule(
                property,
                vm.WhenAnyValue(comparable)
                    .Select(x => x is null || x > value),
                message
                ?? string.Format(
                    ValidationStrings.ShouldBeGreater,
                    format?.Invoke(value) ?? value.ToString()
                )
            );

        public static void GreaterOrEqualRule<TViewModel, TComparable>(
            this TViewModel vm,
            Expression<Func<TViewModel, string?>> property,
            Expression<Func<TViewModel, TComparable?>> comparable,
            TComparable value,
            Func<TComparable?, string>? format = null,
            string? message = null
        )
            where TViewModel : IValidatableForm
            where TComparable : struct, IComparisonOperators<TComparable, TComparable, bool>
            => vm.ValidationRule(
                property,
                vm.WhenAnyValue(comparable)
                    .Select(x => x is null || x >= value),
                message
                ?? string.Format(
                    ValidationStrings.ShouldBeGreaterOrEqual,
                    format?.Invoke(value) ?? value.ToString()
                )
            );

        public static void LessRule<TViewModel, TComparable>(
            this TViewModel vm,
            Expression<Func<TViewModel, string?>> property,
            Expression<Func<TViewModel, TComparable?>> comparable,
            TComparable value,
            Func<TComparable?, string>? format = null,
            string? message = null
        )
            where TViewModel : IValidatableForm
            where TComparable : struct, IComparisonOperators<TComparable, TComparable, bool>
            => vm.ValidationRule(
                property,
                vm.WhenAnyValue(comparable)
                    .Select(x => x is null || x < value),
                message
                ?? string.Format(
                    ValidationStrings.ShouldBeLess,
                    format?.Invoke(value) ?? value.ToString()
                )
            );

        public static void LessOrEqualRule<TViewModel, TComparable>(
            this TViewModel vm,
            Expression<Func<TViewModel, string?>> property,
            Expression<Func<TViewModel, TComparable?>> comparable,
            TComparable value,
            Func<TComparable?, string>? format = null,
            string? message = null
        )
            where TViewModel : IValidatableForm
            where TComparable : struct, IComparisonOperators<TComparable, TComparable, bool>
            => vm.ValidationRule(
                property,
                vm.WhenAnyValue(comparable)
                    .Select(x => x is null || x <= value),
                message
                ?? string.Format(
                    ValidationStrings.ShouldBeLessOrEqual,
                    format?.Invoke(value) ?? value.ToString()
                )
            );
    }
}
