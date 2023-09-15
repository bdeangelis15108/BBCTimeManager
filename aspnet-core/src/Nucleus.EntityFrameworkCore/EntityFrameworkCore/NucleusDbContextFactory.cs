using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Nucleus.Configuration;
using Nucleus.Web;

namespace Nucleus.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class NucleusDbContextFactory : IDesignTimeDbContextFactory<NucleusDbContext>
    {
        public NucleusDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<NucleusDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            NucleusDbContextConfigurer.Configure(builder, configuration.GetConnectionString(NucleusConsts.ConnectionStringName));

            return new NucleusDbContext(builder.Options);
        }
    }
}