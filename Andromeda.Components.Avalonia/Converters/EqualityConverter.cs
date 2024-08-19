using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Andromeda.Components.Avalonia.Converters
{
    public class EqualityConverter : IMultiValueConverter
    {
        public object? Convert(
            IList<object?> values,
            Type targetType,
            object? parameter,
            CultureInfo culture
        )
        {
            if (values.Count < 2)
            {
                return true;
            }

            return values.Skip(1).All(x => x == values[0]);
        }
    }
}
