// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Factory;

internal interface IStorageAdapterFactory
{
    IStorageAdapter CreateStorageAdapter( string url );
}