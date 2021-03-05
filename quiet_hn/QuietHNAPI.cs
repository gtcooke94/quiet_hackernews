using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security.Policy;

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

        public string[] TopItems()
        {
            string url = $"{API_BASE}/topstories.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            var response = request.GetResponse();
            var test = response.ToString();
            var stream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream))
            {
                var test2 = reader.ReadToEnd();
            }



            return new string[] { "test1", "test2" };
        }
    }
}
