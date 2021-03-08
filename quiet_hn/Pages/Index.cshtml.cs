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
        public long RenderTime { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Entries = new List<HackerNewsEntry>();
        }

        public void OnGet()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var hnClient = new QuietHNAPI();
            var entryIds = hnClient.TopItems();
            // Is it better to do this cast or make a new variable with a copy?
            // This servers to garbage collect the longer list that we don't care about?
            //var entryIdsShort = entryIds.Take(35);
            foreach (var id in entryIds)
            {
                var entry = hnClient.GetItemById(id);
                if (entry.type == "story" && !string.IsNullOrEmpty(entry.url))
                {
                    Entries.Add(entry);
                }
                if (Entries.Count == 30)
                {
                    break;
                }
            }
            watch.Stop();
            RenderTime = watch.ElapsedMilliseconds;
        }
    }
}
