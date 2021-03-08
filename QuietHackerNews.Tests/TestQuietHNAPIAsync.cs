using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using quiet_hn;
using quiet_hn.Pages;
using System;
using System.Threading.Tasks;

namespace QuietHackerNews.Tests
{
    [TestClass]
    public class TestQuietHNAPIAsync
    {
        [TestMethod]
        public async Task TestGetTopItemsAsync()
        {
            var test = await new QuietHNAPIAsync().GetTopItemsAsync();
            Console.WriteLine(test);
        }
    }
}
