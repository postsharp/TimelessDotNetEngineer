// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Globalization;
using System.Windows.Data;

namespace Memento.Step3;

internal sealed class SpeciesConverter : IValueConverter
{
    public object Convert( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        if ( value is string s )
        {
            return s switch
            {
                "Scuba Diver" => "🤿",
                _ => (Math.Abs( StringComparer.Ordinal.GetHashCode( s ) ) % 5) switch
                {
                    0 => "🐟",
                    1 => "🐠",
                    2 => "🐡",
                    3 => "🦈",
                    4 => "🐬",
                    _ => throw new NotImplementedException()
                }
            };
        }

        return "";
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture )
    {
        return "";
    }
}