// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using System.Collections;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Polly;

public class ResilientDbDataReader : DbDataReader
{
    private readonly ResiliencePipeline _resiliencePipeline;
    private readonly DbDataReader _underlyingReader;

    public ResilientDbDataReader( DbDataReader underlyingReader, ResiliencePipeline resiliencePipeline )
    {
        this._underlyingReader = underlyingReader;
        this._resiliencePipeline = resiliencePipeline;
    }

    public override object this[ int ordinal ] => this._underlyingReader[ordinal];

    public override object this[ string name ] => this._underlyingReader[name];

    public override int Depth => this._underlyingReader.Depth;

    public override int FieldCount => this._underlyingReader.FieldCount;

    public override bool HasRows => this._underlyingReader.HasRows;

    public override bool IsClosed => this._underlyingReader.IsClosed;

    public override int RecordsAffected => this._underlyingReader.RecordsAffected;

    public override bool GetBoolean( int ordinal )
    {
        return this._underlyingReader.GetBoolean( ordinal );
    }

    public override byte GetByte( int ordinal )
    {
        return this._underlyingReader.GetByte( ordinal );
    }

    public override long GetBytes( int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length )
    {
        return this._underlyingReader.GetBytes( ordinal, dataOffset, buffer, bufferOffset, length );
    }

    public override char GetChar( int ordinal )
    {
        return this._underlyingReader.GetChar( ordinal );
    }

    public override long GetChars( int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length )
    {
        return this._underlyingReader.GetChars( ordinal, dataOffset, buffer, bufferOffset, length );
    }

    public override string GetDataTypeName( int ordinal )
    {
        return this._underlyingReader.GetDataTypeName( ordinal );
    }

    public override DateTime GetDateTime( int ordinal )
    {
        return this._underlyingReader.GetDateTime( ordinal );
    }

    public override decimal GetDecimal( int ordinal )
    {
        return this._underlyingReader.GetDecimal( ordinal );
    }

    public override double GetDouble( int ordinal )
    {
        return this._underlyingReader.GetDouble( ordinal );
    }

    public override IEnumerator GetEnumerator()
    {
        return this._underlyingReader.GetEnumerator();
    }

    [return:
        DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicFields |
            DynamicallyAccessedMemberTypes.PublicProperties )]
    public override Type GetFieldType( int ordinal )
    {
        return this._underlyingReader.GetFieldType( ordinal );
    }

    public override float GetFloat( int ordinal )
    {
        return this._underlyingReader.GetFloat( ordinal );
    }

    public override Guid GetGuid( int ordinal )
    {
        return this._underlyingReader.GetGuid( ordinal );
    }

    public override short GetInt16( int ordinal )
    {
        return this._underlyingReader.GetInt16( ordinal );
    }

    public override int GetInt32( int ordinal )
    {
        return this._underlyingReader.GetInt32( ordinal );
    }

    public override long GetInt64( int ordinal )
    {
        return this._underlyingReader.GetInt64( ordinal );
    }

    public override string GetName( int ordinal )
    {
        return this._underlyingReader.GetName( ordinal );
    }

    public override int GetOrdinal( string name )
    {
        return this._underlyingReader.GetOrdinal( name );
    }

    public override string GetString( int ordinal )
    {
        return this._underlyingReader.GetString( ordinal );
    }

    public override object GetValue( int ordinal )
    {
        return this._underlyingReader.GetValue( ordinal );
    }

    public override int GetValues( object[] values )
    {
        return this._underlyingReader.GetValues( values );
    }

    public override bool IsDBNull( int ordinal )
    {
        return this._underlyingReader.IsDBNull( ordinal );
    }

    public override bool NextResult()
    {
        return this._resiliencePipeline.Execute( () => this._underlyingReader.NextResult() );
    }

    public override bool Read()
    {
        return this._resiliencePipeline.Execute( () => this._underlyingReader.Read() );
    }
}