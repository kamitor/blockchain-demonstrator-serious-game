using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockchainDemonstratorApi.Models.Classes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlockchainDemonstratorApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Count() > 0)
            {
                Config.ServerIp = args[0];
                args = args.Skip(1).ToArray();
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://localhost:5002")
                    .UseStartup<Startup>();
                });
    }
}
