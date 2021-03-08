using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using quiet_hn;
using System;

namespace QuietHackerNews.Tests
{
    [TestClass]
    public class TestJsonConvert
    {
        [TestMethod]
        public void ConvertHNItemTest()
        {
            string json = @"{""by"":""pg"",""descendants"":15,""id"":1,""kids"":[15,234509,487171,454426,454424,454410,82729],""score"":57,""time"":1160418111,""title"":""Y Combinator"",""type"":""story"",""url"":""http://ycombinator.com""}";
            var dynamicJson = JsonConvert.DeserializeObject<dynamic>(json);
            var objectJson = JsonConvert.DeserializeObject<HackerNewsEntry>(json);
        }

        [TestMethod]
        public void APITest()
        {
            var api = new QuietHNAPI();
            var entry = api.GetItemById(1);
            Console.WriteLine("Test");
        }
    }
}
