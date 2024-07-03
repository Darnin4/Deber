using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace AppLogins.Data
{
    public class ProDBContextFactory : IDesignTimeDbContextFactory<ProDBContext>
    {
        public ProDBContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ProDBContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("CadenaSQL"));

            return new ProDBContext(optionsBuilder.Options);
        }
    }
}
