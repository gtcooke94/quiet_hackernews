using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace quiet_hn.QuietHNAPIs
{
    public class HNCache
    {
        public static readonly object _lock = new object();
        public static HNCache _instance = new HNCache();
        private long UpdatedAt { get; set; } = 0;
        private List<HackerNewsEntry> entries = new List<HackerNewsEntry>();
        public int CacheSize { get; set; } = 30;

        public long Timeout = 30;
        public static HNCache Instance
        {
            get
            {
                return _instance;
            }
        }

        public async Task<List<HackerNewsEntry>> GetEntries()
        {
            if (GetTime() - UpdatedAt > Timeout)
            {
                entries = await new QuietHNAPIAsync().GetEntriesAsync(CacheSize);
                UpdatedAt = GetTime();
            }
            return entries;
        }

        private long GetTime()
        {
            return Stopwatch.GetTimestamp() / Stopwatch.Frequency;
        }
    }
}
