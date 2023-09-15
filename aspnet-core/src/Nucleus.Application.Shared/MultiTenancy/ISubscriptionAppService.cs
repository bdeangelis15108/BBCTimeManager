using System.Threading.Tasks;
using Abp.Application.Services;

namespace Nucleus.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
