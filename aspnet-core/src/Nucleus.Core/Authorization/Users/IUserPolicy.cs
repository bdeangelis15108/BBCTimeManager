using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace Nucleus.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
