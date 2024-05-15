// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using PostSharp.Engineering.BuildTools;
using PostSharp.Engineering.BuildTools.Build.Model;
using PostSharp.Engineering.BuildTools.Build.Solutions;
using PostSharp.Engineering.BuildTools.Dependencies.Definitions;
using Spectre.Console.Cli;
using MetalamaDependencies = PostSharp.Engineering.BuildTools.Dependencies.Definitions.MetalamaDependencies.V2024_1;

var product = new Product( MetalamaDependencies.TimelessDotNetEngineer )
{
    Solutions =
    [
        new ManyDotNetSolutions(  "src" ) { IsTestOnly = false, BuildMethod = BuildMethod.Build, CanFormatCode = true }
    ],
    ParametrizedDependencies =
    [
        DevelopmentDependencies.PostSharpEngineering.ToDependency(),
        MetalamaDependencies.MetalamaPatterns.ToDependency()
    ],
};

var commandApp = new CommandApp();

commandApp.AddProductCommands( product );

return commandApp.Run( args );
