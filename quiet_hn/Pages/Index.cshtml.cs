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
            var hnClient = new QuietHNAPI();
            var entryIds = hnClient.TopItems();
            // Is it better to do this cast or make a new variable with a copy?
            // This servers to garbage collect the longer list that we don't care about?
            var entryIdsShort = entryIds.Take(35);
            foreach (var id in entryIdsShort)
            {
                Entries.Add(hnClient.GetItemById(id));
            }
        }
    }
}
