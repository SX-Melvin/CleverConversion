using CleverConversion.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace CleverConversion.Pages
{
    public class ViewModel(IOptions<AppConfiguration> options) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? FileName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? NodeId { get; set; }
        public string BasePath { get; set; }

        public void OnGet()
        {
            BasePath = options.Value.BasePath;
        }
    }
}
