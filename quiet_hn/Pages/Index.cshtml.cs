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
        //private ConcurrentBag<ConcurrencyItem> entryBag;
        public long RenderTime { get; set; }
        const int NUM_ENTRIES = 30;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Entries = new List<HackerNewsEntry>();
            //entryBag = new ConcurrentBag<ConcurrencyItem>();
        }

        public void OnGet()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var hnAPI = new QuietHNAPI();
            Entries = hnAPI.GetEntriesSynchronous(NUM_ENTRIES);
            //GetEntriesParallelFor();
            watch.Stop();
            RenderTime = watch.ElapsedMilliseconds;
        }
    }

    //    public void GetEntriesParallelFor()
    //    {
    //        var hnClient = new QuietHNAPI();
    //        var entryIds = hnClient.TopItems();
    //        // No awaits or anything needed here
    //        Parallel.For(0, NUM_ENTRIES + ENTRY_ADJUSTMENT - 1, i =>
    //        {
    //            var id = entryIds[i];
    //            // Using new API vs. using the captured context API? Since QuietHNAPI doesn't have concurrency, it seems that it would be very bad to try to access in all the different threads?
    //            var entry = new QuietHNAPI().GetItemById(id);
    //            entryBag.Add(new ConcurrencyItem(i, entry));
    //        });
    //        var orderedEntries = new HackerNewsEntry[NUM_ENTRIES + ENTRY_ADJUSTMENT];
    //        foreach(var concurrentItem in entryBag)
    //        {
    //            orderedEntries[concurrentItem.Order] = concurrentItem.HnEntry;
    //        }
    //        foreach(var entry in orderedEntries)
    //        {
    //            maybeAddEntry(entry);
    //            if (Entries.Count == NUM_ENTRIES)
    //            {
    //                break;
    //            }
    //        }
    //    }

    //}

    //public class ConcurrencyItem
    //{
    //    public ConcurrencyItem(int order, HackerNewsEntry hnEntry)
    //    {
    //        Order = order;
    //        HnEntry = hnEntry;
    //    }

    //    public HackerNewsEntry HnEntry { get; set; }
    //    public int Order { get; set; }
    //}
}
