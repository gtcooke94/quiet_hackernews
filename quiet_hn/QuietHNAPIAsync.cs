using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security.Policy;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Concurrent;

namespace quiet_hn
{
    public class QuietHNAPIAsync
    {
        const string API_BASE = "https://hacker-news.firebaseio.com/v0";
        const int ENTRY_ADJUSTMENT = 10;

        public async Task<int[]> GetTopItemsAsync()
        {
            int[] postIds;
            string url = $"{API_BASE}/topstories.json";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                try
                {
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var stringIds = content.Replace("[", "").Replace("]", "");
                    postIds = stringIds.Split(",").Select(int.Parse).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return postIds;
        }

        public async Task<HackerNewsEntry> GetItemById(int id)
        {
            string url = $"{API_BASE}/item/{id}.json";
            HackerNewsEntry entry;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                entry = JsonConvert.DeserializeObject<HackerNewsEntry>(content);
            }
            return entry;
        }

        public async Task<ConcurrencyItem> GetConcurrencyItemById(int id, int order)
        {
            string url = $"{API_BASE}/item/{id}.json";
            HackerNewsEntry entry;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                entry = JsonConvert.DeserializeObject<HackerNewsEntry>(content);
            }
            return new ConcurrencyItem(order, entry);
        }

        public async Task<List<HackerNewsEntry>> GetEntriesAsync(int numToGet)
        {
            var entryIds = await GetTopItemsAsync();
            var entries = new List<HackerNewsEntry>();
            var shortEntryIds = entryIds.Take(numToGet + ENTRY_ADJUSTMENT);
            int i = 0;
            var getEntryTasks = new List<Task<ConcurrencyItem>>();
            foreach(var id in shortEntryIds)
            {
                // Set off a task for each
                // Is there a way I could go on and get them sorted into the ordered array in here with a ContinueWith?
                // Could likely do better with a concurrent dictionary and using i as the key value.
                var entryTask = GetConcurrencyItemById(id, i);
                getEntryTasks.Add(entryTask);
                i++;
            }
            // Await for when all tasks for getting the entry for the id are done
            var allEntries = await Task.WhenAll(getEntryTasks);
            
            var orderedEntries = new HackerNewsEntry[numToGet + ENTRY_ADJUSTMENT];
            foreach (var concurrencyItem in allEntries)
            {
                orderedEntries[concurrencyItem.Order] = concurrencyItem.HnEntry;
            }
            foreach(var entry in orderedEntries)
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
