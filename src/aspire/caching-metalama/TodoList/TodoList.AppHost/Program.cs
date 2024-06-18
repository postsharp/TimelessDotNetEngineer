// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

var builder = DistributedApplication.CreateBuilder( args );

builder.AddProject<Projects.TodoList_Web>("todolist-web");

builder.AddProject<Projects.TodoList_Api>("todolist-api");

builder.Build().Run();
