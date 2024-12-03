// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory;

internal interface IStorageAdapter
{
    Task<Stream> OpenReadAsync();

    Task WriteAsync( Func<Stream, Task> write );
}