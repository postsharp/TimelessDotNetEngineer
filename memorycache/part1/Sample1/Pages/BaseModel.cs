using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sample1.Pages
{
    public class BaseModel : PageModel
    {
        private static readonly string[] _currencies = [
            "bitcoin",
            "ethereum",
            "euro",
            "british-pound-sterling",
        ];

        public string[] Currencies => _currencies;
    }
}
