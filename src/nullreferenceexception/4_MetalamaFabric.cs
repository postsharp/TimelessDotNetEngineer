namespace NullReferenceException.WithMetalama;

// [<snippet body>]
using Metalama.Framework.Fabrics;
using Metalama.Patterns.Contracts;

class Fabric : ProjectFabric
{
    public override void AmendProject(IProjectAmender amender)
        => amender.Outbound.VerifyNotNullableDeclarations();
}
// [<endsnippet body>]