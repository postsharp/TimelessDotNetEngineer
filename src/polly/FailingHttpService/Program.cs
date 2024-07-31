// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

var isOtherTime = false;

var builder = WebApplication.CreateBuilder( args );
var app = builder.Build();

app.MapGet(
    "/FailEveryOtherTime",
    () => (isOtherTime = !isOtherTime) ? Results.StatusCode( 500 ) : Results.Ok( "Hello" ) );

app.Run();