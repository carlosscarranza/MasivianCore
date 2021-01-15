using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Core.Context;
using Microsoft.AspNetCore;
using Utilities.Core.Implementation.Database;

namespace DistributedServices.Core
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        private static int Main(string[] args)
        {
            try
            {
                var configuration = GetConfiguration();

                Console.WriteLine("Configuring web host ({0})...", AppName);
                var host = BuildWebHost(configuration, args);

                Console.WriteLine("Applying migrations ({0})...", AppName);
                host.MigrateDbContext<CoreContext>((context, services) => { });

                Console.WriteLine("Starting web host ({0})...", AppName);
                host.RunAsync().Wait();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Program terminated unexpectedly (ApplicationContext)! {AppName}");
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel(opts => opts.AddServerHeader = false)
                .Build();

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
