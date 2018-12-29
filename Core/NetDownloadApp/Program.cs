using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace NetDownloadApp
{
    class Program
    {
        static int numberOfRequests = 5;

        [DllImport("Kernel32.dll"), SuppressUnmanagedCodeSecurity]
        public static extern int GetCurrentProcessorNumber();

        static async Task Main(string[] args)
        {
            var baseTime = DateTime.Now;

            if (args != null && args.Length > 1)
            {
                numberOfRequests = int.Parse(args[1]);
            }

            if (args != null && args[0] == "async")
            {
                await TestAsync(numberOfRequests, baseTime);
            }
            else if (args[0] == "sync")
            {
                TestSync(numberOfRequests, baseTime);
            }
            else
            {
                TestParallel(numberOfRequests, baseTime);
            }

        }

        static void TestParallel(int numberOfRequests, DateTime baseTime)
        {
            var wc = HttpWebRequest.Create("https://codehaks.com/images/me.jpg");
            wc.Method = "GET";

            var s1 = Stopwatch.StartNew();

            Parallel.For(0, numberOfRequests, index =>
            {
                var start = DateTime.Now;
                wc.GetResponse();
                var duration = (DateTime.Now - start);

                Console.WriteLine($" {index,3:N0} => [ {Thread.CurrentThread.ManagedThreadId,-3:N0}],[ {GetCurrentProcessorNumber(),-2:N0}] == {Math.Ceiling((start - baseTime).TotalMilliseconds),-6:N0} - {Math.Ceiling(duration.TotalMilliseconds),-3:N0}");
            });
            s1.Stop();
            Console.WriteLine($" Total : {s1.ElapsedMilliseconds,3:N0}");


        }


        static void TestParallelAsync(int numberOfRequests, DateTime baseTime)
        {
            var wc = new WebClient();

            var s1 = Stopwatch.StartNew();

            Parallel.For(0, numberOfRequests, async (index) =>
            {
                var start = DateTime.Now;
                await wc.DownloadFileTaskAsync("https://codehaks.com/images/me.jpg", $"download{index}.png");
                var duration = (DateTime.Now - start);

                Console.WriteLine($" {index,3:N0} => [ {Thread.CurrentThread.ManagedThreadId,-3:N0}],[ {GetCurrentProcessorNumber(),-2:N0}] == {Math.Ceiling((start - baseTime).TotalMilliseconds),-6:N0} - {Math.Ceiling(duration.TotalMilliseconds),-3:N0}");
            });
            s1.Stop();
            Console.WriteLine($" Total : {s1.ElapsedMilliseconds,3:N0}");


        }

        static async Task TestAsync(int numberOfRequests, DateTime baseTime)
        {
            var wc = new WebClient();

            var s1 = Stopwatch.StartNew();
            for (int i = 0; i < numberOfRequests; i++)
            {
                var start = DateTime.Now;
                await wc.DownloadFileTaskAsync("https://codehaks.com/images/me.jpg", $"download{i}.png");
                var duration = (DateTime.Now - start);

                Console.WriteLine($" {i,3:N0} => [ {Thread.CurrentThread.ManagedThreadId,-3:N0}],[ {GetCurrentProcessorNumber(),-2:N0}] == {Math.Ceiling((start - baseTime).TotalMilliseconds),-6:N0} - {Math.Ceiling(duration.TotalMilliseconds),-3:N0}");
            }
            s1.Stop();
            Console.WriteLine($" Total : {s1.ElapsedMilliseconds,3:N0}");


        }

        static void TestSync(int numberOfRequests, DateTime baseTime)
        {
            var wc = new WebClient();

            var s1 = Stopwatch.StartNew();
            for (int i = 0; i < numberOfRequests; i++)
            {
                var start = DateTime.Now;
                wc.DownloadFile("https://codehaks.com/images/me.jpg", $"download{i}.png");
                var duration = (DateTime.Now - start);

                Console.WriteLine($" {i,3:N0} => [ {Thread.CurrentThread.ManagedThreadId,-3:N0}],[ {GetCurrentProcessorNumber(),-2:N0}] == {Math.Ceiling((start - baseTime).TotalMilliseconds),-6:N0} - {Math.Ceiling(duration.TotalMilliseconds),-3:N0}");
            }
            s1.Stop();
            Console.WriteLine($" Total : {s1.ElapsedMilliseconds,3:N0}");


        }
    }
}
