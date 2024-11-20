// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

var builder = DistributedApplication.CreateBuilder( args );

var api = builder.AddProject<Projects.TodoList_Api>( "todolist-api" );

builder.AddProject<Projects.TodoList_Web>("todolist-web" )
    .WithExternalHttpEndpoints()
    .WithReference( api );


builder.Build().Run();
