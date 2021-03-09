using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

// TODO, implement cache updates as a separate timed service: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/background-tasks-with-ihostedservice
namespace quiet_hn.QuietHNAPIs
{
    public class HNCache
    {
        public static readonly object _lock = new object();
        public static HNCache _instance = new HNCache();
        private long UpdatedAt { get; set; } = 0;
        private List<HackerNewsEntry> entries = new List<HackerNewsEntry>();
        public int CacheSize { get; set; } = 30;

        public long Timeout = 10;
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
