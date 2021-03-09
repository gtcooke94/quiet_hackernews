using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using quiet_hn;
using quiet_hn.Pages;
using System;
using System.Threading.Tasks;

// These aren't real tests, just using this as a harness to quickly get information on how long these methods are taking

namespace QuietHackerNews.Tests
{
    [TestClass]
    public class TestRuntime 
    {
        [TestMethod]
        public void SyncTest()
        {
            long totalTime = 0;
            int repeats = 5;
            for (var i = 0; i < repeats; i++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                new QuietHNAPI().GetEntriesSynchronous(30);
                watch.Stop();
                var thisTime = watch.ElapsedMilliseconds;
                totalTime += thisTime;
                Console.WriteLine(thisTime);
            }
            Console.WriteLine(totalTime / repeats);
        }
        [TestMethod]
        public void ParallelForTest()
        {
            long totalTime = 0;
            int repeats = 5;
            for (var i = 0; i < repeats; i++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                new QuietHNAPI().GetEntriesParallelFor(30);
                watch.Stop();
                var thisTime = watch.ElapsedMilliseconds;
                totalTime += thisTime;
                Console.WriteLine(thisTime);
            }
            Console.WriteLine(totalTime / repeats);
        }

        [TestMethod]
        public async Task TestGetEntriesAsync()
        {
            long totalTime = 0;
            int repeats = 5;
            for (var i = 0; i < repeats; i++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                await new QuietHNAPIAsync().GetEntriesAsync(30);
                watch.Stop();
                var thisTime = watch.ElapsedMilliseconds;
                totalTime += thisTime;
                Console.WriteLine(thisTime);
            }
            Console.WriteLine(totalTime / repeats);
        }
    }
}
