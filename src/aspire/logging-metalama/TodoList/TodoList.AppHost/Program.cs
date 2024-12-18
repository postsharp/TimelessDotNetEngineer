// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Projects;

var builder = DistributedApplication.CreateBuilder( args );

var api = builder.AddProject<TodoList_Api>( "todolist-api" );

builder.AddProject<TodoList_Web>( "todolist-web" )
    .WithExternalHttpEndpoints()
    .WithReference( api );

builder.Build().Run();