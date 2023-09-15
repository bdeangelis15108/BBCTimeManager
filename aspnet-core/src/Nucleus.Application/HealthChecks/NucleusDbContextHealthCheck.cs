using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nucleus.EntityFrameworkCore;

namespace Nucleus.HealthChecks
{
    public class NucleusDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public NucleusDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("NucleusDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("NucleusDbContext could not connect to database"));
        }
    }
}
