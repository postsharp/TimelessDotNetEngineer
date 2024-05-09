using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sample1.Pages;

public class BaseModel : PageModel
{
    // [<snippet currencies>]
    public IReadOnlyList<string> Currencies { get; } =
    [
        "bitcoin",
        "ethereum",
        "euro",
        "british-pound-sterling"
    ];
    // [<endsnippet currencies>]
}