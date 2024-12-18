// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using OpenTelemetry;
using OpenTelemetry.Logs;
using System.Collections;
using System.Globalization;
using System.Text;

internal class CustomLoggingProcessor : BaseProcessor<LogRecord>
{
    public override void OnEnd( LogRecord logRecord )
    {
        if ( logRecord.Attributes != null )
        {
            logRecord.Attributes = new CustomLoggingEnumerator( logRecord.Attributes );
        }
    }

    private sealed class CustomLoggingEnumerator : IReadOnlyList<KeyValuePair<string, object?>>
    {
        private readonly IReadOnlyList<KeyValuePair<string, object?>> _state;

        public CustomLoggingEnumerator( IReadOnlyList<KeyValuePair<string, object?>> state )
        {
            this._state = state;
        }

        public int Count => this._state.Count;

        public KeyValuePair<string, object?> this[ int index ]
        {
            get
            {
                var item = this._state[index];
                var key = item.Key;
                var value = SafeSerializeObject( item.Value );

                return new KeyValuePair<string, object?>( key, value );
            }
        }

        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        {
            for ( var i = 0; i < this.Count; i++ )
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static string SafeSerializeObject( object? value, int depth = 0 )
        {
            if ( value is string )
            {
                return $"\"{value}\"";
            }

            var type = value?.GetType();

            if ( value == null || depth > 5 || type == null || type.IsPrimitive )
            {
                return value?.ToString() ?? "null";
            }

            var builder = new StringBuilder();

            if ( type != null )
            {
                builder.Append( CultureInfo.CurrentCulture, $"<{type.Name}> " );
            }

            if ( value is DateTime )
            {
                builder.Append( CultureInfo.CurrentCulture, $"\"{value:o}\"" );
            }
            else if ( value is ICollection collection )
            {
                SerializeEnumerable( collection, depth, builder );
            }
            else if ( value is CancellationToken ) { }
            else
            {
                SerializeComplexObject( value, depth, builder );
            }

            return builder.ToString();
        }

        private static void SerializeEnumerable( IEnumerable enumerable, int depth, StringBuilder builder )
        {
            builder.Append( "[ " );

            var isFirst = true;

            foreach ( var item in enumerable )
            {
                if ( !isFirst )
                {
                    builder.Append( ", " );
                }

                builder.Append( SafeSerializeObject( item, depth + 1 ) );
                isFirst = false;
            }

            builder.Append( " ]" );
        }

        private static void SerializeComplexObject( object value, int depth, StringBuilder builder )
        {
            builder.Append( "{ " );
            var isFirst = true;

            foreach ( var property in value.GetType().GetProperties() )
            {
                if ( !isFirst )
                {
                    builder.Append( ", " );
                }

                builder.Append( CultureInfo.CurrentCulture, $"\"{property.Name}\": " );

                try
                {
                    var propertyValue = property.GetValue( value );
                    builder.Append( SafeSerializeObject( propertyValue, depth + 1 ) );
                }
                catch
                {
                    builder.Append( "\"[Error reading property]\"" );
                }

                isFirst = false;
            }

            builder.Append( " }" );
        }
    }
}