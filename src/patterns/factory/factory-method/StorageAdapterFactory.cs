// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Factory
{
    internal sealed class StorageAdapterFactory
    {
        public static IStorageAdapter CreateStorageAdapter( string pathOrUrl )
        {
            // Logic to determine storage adapter type
            if ( pathOrUrl.StartsWith( "https://" ) )
            {
                return new HttpStorageAdapter( pathOrUrl );
            }

            return new FileSystemStorageAdapter( pathOrUrl );
        }
    }
}