using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiet_hn.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<HackerNewsEntry> Entries;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Entries = new List<HackerNewsEntry>();
        }

        public void OnGet()
        {
            Entries.Add(new HackerNewsEntry("Test Entry"));
            var hn_client = new QuietHNAPI();
            hn_client.TopItems();
            hn_client.GetItemById(1);
        }
    }
}
