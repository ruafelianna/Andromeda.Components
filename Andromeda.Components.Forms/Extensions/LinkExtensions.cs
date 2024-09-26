using Andromeda.Components.Forms.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Andromeda.Components.Forms.Extensions
{
    public static class LinkExtensions
    {
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
