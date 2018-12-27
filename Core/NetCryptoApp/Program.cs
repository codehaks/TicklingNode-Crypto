using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace NetCryptoApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            byte[] salt = Encoding.ASCII.GetBytes("salt");

            const int numberOfRequests = 100;
            var baseTime = DateTime.Now;

            if (args != null && args[0] == "async")
            {
                await TestAsync(salt, numberOfRequests, baseTime);
            }
            else
            {
                Test(salt, numberOfRequests, baseTime);
            }
        }

        private static async Task TestAsync(byte[] salt, int numberOfRequests, DateTime baseTime)
        {
            ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
            Console.WriteLine($" Worker threads : {workerThreads}");
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

                //ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
                //Console.WriteLine($" Worker threads : {workerThreads}");

                Console.WriteLine($"{i} => [{threadId.ToString()}] == {Math.Ceiling((start - baseTime).TotalMilliseconds)} - {Math.Ceiling(duration.TotalMilliseconds)}");

            }
        }

        private static void Test(byte[] salt, int numberOfRequests, DateTime baseTime)
        {
            ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
            Console.WriteLine($" Worker threads : {workerThreads}");

            for (int i = 1; i < numberOfRequests + 1; i++)
            {
                var start = DateTime.Now;
                KeyDerivation.Pbkdf2("password", salt, KeyDerivationPrf.HMACSHA512, 100000, 512);
                var duration = (DateTime.Now - start);

                Console.WriteLine($"{i} => [{Thread.CurrentThread.ManagedThreadId}] == {Math.Ceiling((start - baseTime).TotalMilliseconds)} - {Math.Ceiling(duration.TotalMilliseconds)}");
            }
        }



        public static string CryptoPbkdf2(byte[] salt, out int threadId)
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            return KeyDerivation.Pbkdf2("password", salt, KeyDerivationPrf.HMACSHA512, 10000, 512).ToString();


        }
    }
}
