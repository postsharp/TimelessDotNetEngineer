using Metalama.Patterns.Observability;

namespace Memento.Step3;

// [<snippet Type>]
[Memento]
[Observable]
public sealed partial class Fish 
{
    public string? Name { get; set; }

    public string? Species { get; set; }

    public DateTime DateAdded { get; set; }
}

// [<endsnippet Type>]