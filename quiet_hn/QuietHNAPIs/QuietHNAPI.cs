using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security.Policy;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace quiet_hn
{
    public class QuietHNAPI
    {
        const string API_BASE = "https://hacker-news.firebaseio.com/v0";
        const int ENTRY_ADJUSTMENT = 10;

        public int[] TopItems()
        {
            string url = $"{API_BASE}/topstories.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            var response = request.GetResponse();
            var test = response.ToString();
            var stream = response.GetResponseStream();
            string content;
            using (StreamReader reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }
            var stringIds = content.Replace("[", "").Replace("]", "");
            int[] postIds = stringIds.Split(",").Select(int.Parse).ToArray();

            return postIds;
        }

        public HackerNewsEntry GetItemById(int id)
        {
            string url = $"{API_BASE}/item/{id}.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            var response = request.GetResponse();
            var test = response.ToString();
            var stream = response.GetResponseStream();
            string content;
            using (StreamReader reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }
            HackerNewsEntry entry = JsonConvert.DeserializeObject<HackerNewsEntry>(content);
            return entry;
        }
        public List<HackerNewsEntry> GetEntriesSynchronous(int numToGet)
        {
            var entries = new List<HackerNewsEntry>(30);
            var entryIds = TopItems();
            // Is it better to do this cast or make a new variable with a copy?
            // This servers to garbage collect the longer list that we don't care about?
            //var entryIdsShort = entryIds.Take(35);
            foreach (var id in entryIds)
            {
                var entry = GetItemById(id);
                if (IsValidEntry(entry))
                {
                    entries.Add(entry);
                }
                if (entries.Count == numToGet)
                {
                    break;
                }
            }
            return entries;
        }



        public List<HackerNewsEntry> GetEntriesParallelFor(int numToGet)
        {
            var entryIds = TopItems();
            var entries = new List<HackerNewsEntry>(30);
            var entryBag = new ConcurrentBag<ConcurrencyItem>();
            // No awaits or anything needed here
            Parallel.For(0, numToGet + ENTRY_ADJUSTMENT - 1, i =>
            {
                var id = entryIds[i];
                // Using new API vs. using the captured context API? Since QuietHNAPI doesn't have concurrency, it seems that it would be very bad to try to access in all the different threads?
                var entry = new QuietHNAPI().GetItemById(id);
                entryBag.Add(new ConcurrencyItem(i, entry));
            });
            var orderedEntries = new HackerNewsEntry[numToGet + ENTRY_ADJUSTMENT];
            foreach (var concurrentItem in entryBag)
            {
                orderedEntries[concurrentItem.Order] = concurrentItem.HnEntry;
            }
            foreach (var entry in orderedEntries)
            {
                if (IsValidEntry(entry))
                {
                    entries.Add(entry);
                }
                if (entries.Count == numToGet)
                {
                    break;
                }
            }
            return entries;
        }
        private bool IsValidEntry(HackerNewsEntry entry)
        {
            return entry.type == "story" && !string.IsNullOrEmpty(entry.url);
        }
   }
}

