// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class DefaultHttpClientFactory : IHttpClientFactory
    {
        public DefaultHttpClientFactory() { }

        public HttpClient CreateClient( string name )
        {
            throw new NotImplementedException();
        }
    }
}