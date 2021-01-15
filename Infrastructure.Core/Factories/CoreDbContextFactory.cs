using System;
using System.IO;
using System.Linq;
using Infrastructure.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Core.Factories
{
    public class CoreDbContextFactory : IDesignTimeDbContextFactory<CoreContext>                     
    {
        public CoreContext CreateDbContext(string[] args)
        {
            var coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(CoreDbContextFactory).Assembly.Location);
            if (coreAssemblyDirectoryPath == null)
            {
                throw new Exception("Could not find location! " + typeof(CoreDbContextFactory).FullName);
            }
            var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
            Console.WriteLine(directoryInfo.FullName);
            while (!DirectoryContains(directoryInfo.FullName))
            {
                directoryInfo = directoryInfo.Parent ?? throw new Exception("Could not find content DistributedServices.Core folder");
            }
            Console.WriteLine(directoryInfo.FullName);
            var webHostFolder = Path.Combine(directoryInfo.FullName, "MasivianCore", "DistributedServices.Core");

            if (!Directory.Exists(webHostFolder))
            {
                throw new Exception("Could not find root folder of the web project!");
            }

            var configuration = AppConfigurations.Get(webHostFolder);

            var optionsBuilder = new DbContextOptionsBuilder<CoreContext>();

            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:Default"],
                sqlServerOptionsAction: o => o.MigrationsAssembly("Infrastructure.Core")
            );
            return new CoreContext(optionsBuilder.Options);
        }

        private static bool DirectoryContains(string directory)
        {
            return Directory.GetDirectories(directory).Any(filePath => {
                var webHostFolder = Path.Combine(filePath, "DistributedServices.Core");
                return Directory.Exists(webHostFolder);
            });
        }
    }
}
