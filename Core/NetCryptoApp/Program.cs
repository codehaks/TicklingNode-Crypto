using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;
using System.Threading.Tasks;
namespace NetCryptoApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            byte[] salt = Encoding.ASCII.GetBytes("salt");

            const int numberOfRequests = 10;
            var baseTime = DateTime.Now;

            for (int i = 1; i < numberOfRequests + 1; i++)
            {
                var start = DateTime.Now;

                var task = Task.Run(() => KeyDerivation.Pbkdf2("password", salt, KeyDerivationPrf.HMACSHA512, 10000, 512));

                await task;

                var duration = (DateTime.Now - start);
                Console.WriteLine($"{i} => {Math.Ceiling((start - baseTime).TotalMilliseconds)} - {Math.Ceiling(duration.TotalMilliseconds)}");

            }
        }

        private Task Pbkdf2Async(byte[] salt)
        {
            KeyDerivation.Pbkdf2("password", salt, KeyDerivationPrf.HMACSHA512, 10000, 512);
            return Task.CompletedTask;
        }
    }
}
