using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TicketHive.Infrastructure.Persistence.DesignTime
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var currentDir = Directory.GetCurrentDirectory();

            // Try to resolve solution root from Infrastructure project directory
            var srcDir = Directory.GetParent(currentDir);              // .../src
            var solutionRoot = srcDir?.Parent ?? new DirectoryInfo(currentDir);
            var apiConfigDir = Path.Combine(solutionRoot.FullName, "src", "TicketHive.Api");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiConfigDir)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("DefaultConnection is not configured. Provide it via appsettings or ConnectionStrings__DefaultConnection env var.");
            }
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}


