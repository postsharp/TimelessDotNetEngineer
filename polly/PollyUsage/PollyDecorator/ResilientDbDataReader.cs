using Polly;
using System.Collections;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace PollyDecorator;

public class ResilientDbDataReader : DbDataReader
{
    private readonly DbDataReader _underlyingReader;
    private readonly ResiliencePipeline _resiliencePipeline;

    public ResilientDbDataReader(DbDataReader underlyingReader, ResiliencePipeline resiliencePipeline)
    {
        _underlyingReader = underlyingReader;
        _resiliencePipeline = resiliencePipeline;
    }

    public override object this[int ordinal] => _underlyingReader[ordinal];

    public override object this[string name] => _underlyingReader[name];

    public override int Depth => _underlyingReader.Depth;

    public override int FieldCount => _underlyingReader.FieldCount;

    public override bool HasRows => _underlyingReader.HasRows;

    public override bool IsClosed => _underlyingReader.IsClosed;

    public override int RecordsAffected => _underlyingReader.RecordsAffected;

    public override bool GetBoolean(int ordinal) => _underlyingReader.GetBoolean(ordinal);

    public override byte GetByte(int ordinal) => _underlyingReader.GetByte(ordinal);

    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length) => _underlyingReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);

    public override char GetChar(int ordinal) => _underlyingReader.GetChar(ordinal);

    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length) => _underlyingReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);

    public override string GetDataTypeName(int ordinal) => _underlyingReader.GetDataTypeName(ordinal);

    public override DateTime GetDateTime(int ordinal) => _underlyingReader.GetDateTime(ordinal);

    public override decimal GetDecimal(int ordinal) => _underlyingReader.GetDecimal(ordinal);

    public override double GetDouble(int ordinal) => _underlyingReader.GetDouble(ordinal);

    public override IEnumerator GetEnumerator() => _underlyingReader.GetEnumerator();

    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
    public override Type GetFieldType(int ordinal) => _underlyingReader.GetFieldType(ordinal);

    public override float GetFloat(int ordinal) => _underlyingReader.GetFloat(ordinal);

    public override Guid GetGuid(int ordinal) => _underlyingReader.GetGuid(ordinal);

    public override short GetInt16(int ordinal) => _underlyingReader.GetInt16(ordinal);

    public override int GetInt32(int ordinal) => _underlyingReader.GetInt32(ordinal);

    public override long GetInt64(int ordinal) => _underlyingReader.GetInt64(ordinal);

    public override string GetName(int ordinal) => _underlyingReader.GetName(ordinal);

    public override int GetOrdinal(string name) => _underlyingReader.GetOrdinal(name);

    public override string GetString(int ordinal) => _underlyingReader.GetString(ordinal);

    public override object GetValue(int ordinal) => _underlyingReader.GetValue(ordinal);

    public override int GetValues(object[] values) => _underlyingReader.GetValues(values);

    public override bool IsDBNull(int ordinal) => _underlyingReader.IsDBNull(ordinal);

    public override bool NextResult()
        => this._resiliencePipeline.Execute(() => _underlyingReader.NextResult());

    public override bool Read()
        => this._resiliencePipeline.Execute(() => _underlyingReader.Read());
}
