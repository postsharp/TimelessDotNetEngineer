// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

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