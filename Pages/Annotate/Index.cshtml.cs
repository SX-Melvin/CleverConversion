using CleverConversion.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace CleverConversion.Pages.Annotation
{
    public class IndexModel(IOptions<AppConfiguration> options) : PageModel
    {
        public string BasePath { get; set; } = options.Value.BasePath;
        public void OnGet()
        {
        }
    }
}
