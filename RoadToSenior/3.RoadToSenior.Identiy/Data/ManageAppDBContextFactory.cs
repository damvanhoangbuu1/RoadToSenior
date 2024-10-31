using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace _3.RoadToSenior.Identiy.Data
{
    public class ManageAppDBContextFactory : IDesignTimeDbContextFactory<ManageAppDBContext>
    {
        public ManageAppDBContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ManageAppDBContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseNpgsql(connectionString);

            return new ManageAppDBContext(builder.Options);
        }
    }
}
