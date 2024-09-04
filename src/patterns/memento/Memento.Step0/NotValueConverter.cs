// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Globalization;
using System.Windows.Data;

namespace Memento.Step0;

internal sealed class NotValueConverter : IValueConverter
{
    public object Convert( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        if ( value is bool b )
        {
            return !b;
        }

        return false;
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture )
    {
        if ( value is bool b )
        {
            return !b;
        }

        return true;
    }
}