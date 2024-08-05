// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Projects;

var builder = DistributedApplication.CreateBuilder( args );

var db = builder
    .AddSqlServer( "dbengine" )
    .AddDatabase( "database" );

var cache = builder.AddRedis( "cache" );

var apiService = builder
    .AddProject<OutdoorTodoList_ApiService>( "apiservice" )
    .WithReference( db )
    .WithReference( cache );

builder.AddProject<OutdoorTodoList_Web>( "webfrontend" )
    .WithExternalHttpEndpoints()
    .WithReference( cache )
    .WithReference( apiService );

builder.Build().Run();