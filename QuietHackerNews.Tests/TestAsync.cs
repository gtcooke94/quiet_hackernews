using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using quiet_hn;
using quiet_hn.Pages;
using System;

namespace QuietHackerNews.Tests
{
    [TestClass]
    public class TestAsyncAPI
    {
        [TestMethod]
        public async void TestGetTopItemsAsync()
        {
            var test = await QuietHNAPIAsync.GetTopItemsAsync();
        }
    }
}
