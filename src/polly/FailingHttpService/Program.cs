// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

var isOtherTime = false;

var builder = WebApplication.CreateBuilder( args );
var app = builder.Build();

app.MapGet( "/FailEveryOtherTime", () => (isOtherTime = !isOtherTime) ? Results.StatusCode( 500 ) : Results.Ok( "Hello" ) );

app.Run();