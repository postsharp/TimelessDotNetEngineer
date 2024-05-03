using Metalama.Extensions.Architecture.Fabrics;
using Metalama.Extensions.Architecture.Predicates;
using Metalama.Framework.Fabrics;

namespace PollyDependencyInjection;

internal class AvoidInstantiatingHttpClientFabric : ProjectFabric
{
    public override void AmendProject(IProjectAmender amender)
    {
        amender.Verify().SelectTypes(typeof(HttpClient)).SelectMany(t => t.Constructors).CannotBeUsedFrom(r => r.Always(), $"Use {nameof(IHttpClientFactory)} instead.");
    }
}
