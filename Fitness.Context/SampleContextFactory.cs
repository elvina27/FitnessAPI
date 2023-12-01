using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fitness.Context
{
    public class SampleContextFactory : IDesignTimeDbContextFactory<FitnessContext>
    {
        public FitnessContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<FitnessContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new FitnessContext(options);
        }
    }
}
