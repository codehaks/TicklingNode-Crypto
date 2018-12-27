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
                var caller = new Pbkdf2Async(CryptoPbkdf2);

                IAsyncResult task = caller.BeginInvoke(salt, null, null);
                var result = caller.EndInvoke(task);

                var duration = (DateTime.Now - start);
                Console.WriteLine($"{i} => {Math.Ceiling((start - baseTime).TotalMilliseconds)} - {Math.Ceiling(duration.TotalMilliseconds)}");

            }
        }

        public delegate string Pbkdf2Async(byte[] salt);

        public static string CryptoPbkdf2(byte[] salt)
        {
            return KeyDerivation.Pbkdf2("password", salt, KeyDerivationPrf.HMACSHA512, 10000, 512).ToString();
      
        }
    }
}
