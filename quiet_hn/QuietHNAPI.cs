using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security.Policy;
using Newtonsoft.Json;

namespace quiet_hn
{
    public class QuietHNAPI
    {
        public static string BasicGet()
        {
            string url = "http://www.google.com";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            var response = request.GetResponse();
            var test = response.ToString();
            var stream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream))
            {
                var test2 = reader.ReadToEnd();
            }
            return "";
        }

        const string API_BASE = "https://hacker-news.firebaseio.com/v0";

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
    }

    public class HackerNewsEntry
    {
        public string by;
        public string decesdants;
        public int id;
        public int[] kids;
        public int score;
        public int time;
        public string title;
        public string type;
        public string url;

        public HackerNewsEntry(string title)
        //{ "by":"pg","descendants":15,"id":1,"kids":[15,234509,487171,454426,454424,454410,82729],"score":57,"time":1160418111,"title":"Y Combinator","type":"story","url":"http://ycombinator.com"}
        {
        }


        public string Title { get; }
    }
}
