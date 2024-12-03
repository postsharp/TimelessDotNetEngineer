// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Factory;

internal interface IStorageAdapter
{
    Task<Stream> OpenReadAsync();

    Task WriteAsync( Func<Stream, Task> write );
}