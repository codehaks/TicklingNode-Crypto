using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace CryptoWeb
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            byte[] salt = Encoding.ASCII.GetBytes("salt");

            const int numberOfRequests = 10;



            app.Run(async (context) =>
            {
                for (int i = 1; i < numberOfRequests + 1; i++)
                {
                    var start = DateTime.Now;
                    KeyDerivation.Pbkdf2("password", salt, KeyDerivationPrf.HMACSHA512, 10000, 512);
                    var duration = (DateTime.Now - start);
                    //Console.WriteLine($"{i} => {duration.TotalMilliseconds}");
                    await context.Response.WriteAsync($"{i} => {duration.TotalMilliseconds}");
                }

            });
        }
    }
}
