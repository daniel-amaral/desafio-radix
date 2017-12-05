using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace DesafioRadix
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            uint retries = 36;
            int delayInMiliseconds = 5 * 1000;

            while (retries > 0)
            {
                try
                {
                    var webHost = WebHost.CreateDefaultBuilder(args);
                    webHost.UseStartup<Startup>();
                    return webHost.Build();
                }
                #pragma warning disable CS0168 // Variable is declared but never used
                catch (Exception _)
                {
                    Console.WriteLine("Database connection refused. " + retries-- + " retries left...");
                    System.Threading.Thread.Sleep(delayInMiliseconds);
                }
            }

            Console.WriteLine("Database connection timeout");

            return WebHost.CreateDefaultBuilder(args)
                    .CaptureStartupErrors(true)
                    .UseStartup<Startup>()
                    .Build();

        }
    }
}
