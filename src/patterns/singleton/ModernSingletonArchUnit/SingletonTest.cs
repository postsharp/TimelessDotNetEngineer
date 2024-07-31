// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

// [<snippet body>]

using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

public class SingletonTest
{
    private static readonly Architecture _architecture = new ArchLoader()
        .LoadAssemblies( typeof(PerformanceCounterManager).Assembly )
        .Build();

    private static readonly IObjectProvider<Class> _singletons =
        Classes().That().HaveAnyAttributes( typeof(SingletonAttribute) ).As( "singletons" );

    [Fact]
    public void SingletonContructorCannnotBeAccessed()
    {
        Types()
            .Should()
            .NotCallAny(
                MethodMembers().That().AreConstructors().And().AreDeclaredIn( _singletons ) )
            .Check( _architecture );
    }
}

// [<endsnippet body>]