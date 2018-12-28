using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace NetCryptoApp
{
    class Program
    {
        static int numberOfRequests = 10;
        static int iterationCount = 10000;

        static async Task Main(string[] args)
        {
            byte[] salt = Encoding.ASCII.GetBytes("salt");

            var baseTime = DateTime.Now;

            if (args!=null && args.Length>1)
            {
                numberOfRequests = int.Parse(args[1]);
            }

            if (args != null && args.Length > 2)
            {
                iterationCount = int.Parse(args[2]);
            }

            if (args != null && args[0] == "async")
            {
                await TestAsync(salt, numberOfRequests, baseTime);
            }
            else if (args[0] == "sync")
            {
                TestSync(salt, numberOfRequests, baseTime);
            }
            else
            {
                TestParallel(salt, numberOfRequests, baseTime);
            }
        }

        private static async Task TestAsync(byte[] salt, int numberOfRequests, DateTime baseTime)
        {
            ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
            Console.WriteLine($" Worker threads : {workerThreads}");
            var s1 = Stopwatch.StartNew();
            for (int i = 1; i < numberOfRequests + 1; i++)
            {
                var start = DateTime.Now;
                int threadId = 0;
                await Task.Run(() =>
                {
                    CryptoPbkdf2(salt, out int cthreadId);
                    threadId = cthreadId;
                });
                var duration = (DateTime.Now - start);
                Console.WriteLine($" {i,3:N0} => [  {Thread.CurrentThread.ManagedThreadId,-3:N0}] == {Math.Ceiling((start - baseTime).TotalMilliseconds),-6:N0} - {Math.Ceiling(duration.TotalMilliseconds),-3:N0}");
            }
            s1.Stop();
            Console.WriteLine($" Total : {s1.ElapsedMilliseconds,3:N0}");
        }

        private static void TestSync(byte[] salt, int numberOfRequests, DateTime baseTime)
        {
            ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
            Console.WriteLine($" Worker threads : {workerThreads}");

            var s1 = Stopwatch.StartNew();
            for (int i = 1; i < numberOfRequests + 1; i++)
            {
                var start = DateTime.Now;
                KeyDerivation.Pbkdf2("password", salt, KeyDerivationPrf.HMACSHA512, iterationCount, 512);
                var duration = (DateTime.Now - start);

                Console.WriteLine($" {i,3:N0} => [  {Thread.CurrentThread.ManagedThreadId,-3:N0}] == {Math.Ceiling((start - baseTime).TotalMilliseconds),-6:N0} - {Math.Ceiling(duration.TotalMilliseconds),-3:N0}");
            }
            s1.Stop();
            Console.WriteLine($" Total : {s1.ElapsedMilliseconds,3:N0}");
        }

        private static void TestParallel(byte[] salt, int numberOfRequests, DateTime baseTime)
        {
            ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
            Console.WriteLine($" Worker threads : {workerThreads}");

            var s1 = Stopwatch.StartNew();

            Parallel.For(0, numberOfRequests, index =>
            {
                var start = DateTime.Now;
                KeyDerivation.Pbkdf2("password", salt, KeyDerivationPrf.HMACSHA512, iterationCount, 512);
                var duration = (DateTime.Now - start);
                Console.WriteLine($" {index,3:N0} => [  {Thread.CurrentThread.ManagedThreadId,-3:N0}] == {Math.Ceiling((start - baseTime).TotalMilliseconds),-6:N0} - {Math.Ceiling(duration.TotalMilliseconds),-3:N0}");
            });

            s1.Stop();
            Console.WriteLine($" Total : {s1.ElapsedMilliseconds,3:N0}");
            
        }

        public static void CryptoPbkdf2(byte[] salt, out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            KeyDerivation.Pbkdf2("password", salt, KeyDerivationPrf.HMACSHA512, iterationCount, 512).ToString();
        }
    }
}
