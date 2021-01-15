using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Core
{
    public static class AppConfigurations
    {
        private static readonly ConcurrentDictionary<string, IConfigurationRoot> ConfigurationCache;

        static AppConfigurations()
        {
            ConfigurationCache = new ConcurrentDictionary<string, IConfigurationRoot>();
        }

        public static IConfigurationRoot Get(string path, string environmentName = null)
        {
            var cacheKey = path + "#" + environmentName;
            return ConfigurationCache.GetOrAdd(
                cacheKey,
                _ => BuildConfiguration(path, environmentName)
            );
        }

        private static IConfigurationRoot BuildConfiguration(string path, string environmentName = null)
        {
            Console.WriteLine(path);
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                builder = builder.AddJsonFile($"{environmentName}.json", optional: true);
            }

            builder = builder.AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
