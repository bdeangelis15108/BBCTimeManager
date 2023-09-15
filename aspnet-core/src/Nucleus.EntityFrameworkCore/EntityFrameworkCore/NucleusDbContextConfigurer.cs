using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.EntityFrameworkCore
{
    public static class NucleusDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<NucleusDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<NucleusDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}