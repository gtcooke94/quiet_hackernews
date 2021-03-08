using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiet_hn.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<HackerNewsEntry> Entries;
        public string ExtraMessage = null;
        //private ConcurrentBag<ConcurrencyItem> entryBag;
        public long RenderTime { get; set; }
        const int NUM_ENTRIES = 30;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Entries = new List<HackerNewsEntry>();
            //entryBag = new ConcurrentBag<ConcurrencyItem>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //Entries = hnAPI.GetEntriesSynchronous(NUM_ENTRIES);

            //Entries = new QuietHNAPI().GetEntriesParallelFor(NUM_ENTRIES);
            
            Entries = await new QuietHNAPIAsync().GetEntriesAsync(NUM_ENTRIES);
            watch.Stop();
            RenderTime = watch.ElapsedMilliseconds;
            return Page();
        }
    }
}
