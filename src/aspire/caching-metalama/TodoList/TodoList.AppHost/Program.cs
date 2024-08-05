// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Projects;

var builder = DistributedApplication.CreateBuilder( args );

var db = builder
    .AddSqlServer( "dbengine" )
    .AddDatabase( "database" );

// [<snippet AppHostCacheConfiguration>]
var cache = builder
    .AddRedis( "cache" );

var api = builder
    .AddProject<TodoList_Api>( "todolist-api" )
    .WithReference( db )
    .WithReference( cache );

// [<endsnippet AppHostCacheConfiguration>]

builder.AddProject<TodoList_Web>( "todolist-web" )
    .WithExternalHttpEndpoints()
    .WithReference( api );

builder.Build().Run();