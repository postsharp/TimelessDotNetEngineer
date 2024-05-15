// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using LoggingWithInterpolation.WithMetalama;
using Microsoft.Extensions.Logging;
using System.Text.Json;

var logJson = false;

using var factory = LoggerFactory.Create(
    builder =>
    {
        if ( logJson )
        {
            builder.AddJsonConsole( options => options.JsonWriterOptions = new JsonWriterOptions { Indented = true } );
        }
        else
        {
            builder.AddSimpleConsole();
        }
    } );

var logger = factory.CreateLogger<Shop>();

var shop = new Shop( logger );
shop.Sell( new Product( "llama T-shirt" ), 10.0m );