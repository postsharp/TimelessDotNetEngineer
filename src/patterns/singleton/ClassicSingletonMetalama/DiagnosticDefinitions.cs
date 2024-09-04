using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;

namespace ClassicSingletonMetalama;

[CompileTime]
internal static class DiagnosticDefinitions
{
    public static readonly DiagnosticDefinition<INamedType> SingletonCannotDefineConstructor =
        new(
            "SINGLE01",
            Severity.Error,
            "The type '{0}' cannot define public or internal constructors because it is a singleton." );
}