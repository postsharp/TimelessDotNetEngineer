// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

internal class BufferingResponseCookies : IResponseCookies
{
    private readonly List<Action<IResponseCookies>> _operations = new();

    public void Append( string key, string value )
    {
        this._operations.Add( cookies => cookies.Append( key, value ) );
    }

    public void Append( string key, string value, CookieOptions options )
    {
        this._operations.Add( cookies => cookies.Append( key, value, options ) );
    }

    public void Delete( string key )
    {
        this._operations.Add( cookies => cookies.Delete( key ) );
    }

    public void Delete( string key, CookieOptions options )
    {
        this._operations.Add( cookies => cookies.Delete( key, options ) );
    }

    public void Reset()
    {
        this._operations.Clear();
    }

    public void Accept( IResponseCookies cookies )
    {
        foreach ( var operation in this._operations )
        {
            operation( cookies );
        }
    }
}