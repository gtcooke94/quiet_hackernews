using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security.Policy;
using Newtonsoft.Json;
using System.Net.Http;

namespace quiet_hn
{
    public class QuietHNAPIAsync
    {
        const string API_BASE = "https://hacker-news.firebaseio.com/v0";

        public async static Task<int[]> GetTopItemsAsync()
        {
            int[] postIds;
            using (var client = new HttpClient())
            {
                string url = $"{API_BASE}/topstories.json";
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
    }
}
