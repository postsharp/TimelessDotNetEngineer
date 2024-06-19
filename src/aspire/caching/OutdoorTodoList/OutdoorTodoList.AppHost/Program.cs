// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

var builder = DistributedApplication.CreateBuilder( args );

var db = builder
    .AddSqlServer( "dbengine" )
    .AddDatabase( "database" );

var cache = builder.AddRedis( "cache" );

var apiService = builder
    .AddProject<Projects.OutdoorTodoList_ApiService>( "apiservice" )
    .WithReference( db )
    .WithReference( cache );

builder.AddProject<Projects.OutdoorTodoList_Web>( "webfrontend" )
    .WithExternalHttpEndpoints()
    .WithReference( cache )
    .WithReference( apiService );

builder.Build().Run();
