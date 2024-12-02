// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory;

class TestStorageAdapter : IStorageAdapter
{
    private byte[] _buffer = [];

    public Task<Stream> OpenReadAsync()
    {
        return Task.FromResult<Stream>(new MemoryStream(this._buffer));
    }

    public async Task WriteAsync(Func<Stream, Task> write)
    {
        using var memoryStream = new MemoryStream();
        await write(memoryStream);
        this._buffer = memoryStream.ToArray();
    }
}
