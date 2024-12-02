// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory;

class FileSystemStorageAdapter : IStorageAdapter
{
    private readonly string _filePath;

    public FileSystemStorageAdapter(string filePath)
    {
        _filePath = filePath;
    }

    public Task<Stream> OpenReadAsync() =>
        Task.FromResult((Stream)File.OpenRead(_filePath));

    public async Task WriteAsync(Func<Stream, Task> write)
    {
        await using var stream = File.OpenWrite(_filePath);
        await write(stream);
    }
}
