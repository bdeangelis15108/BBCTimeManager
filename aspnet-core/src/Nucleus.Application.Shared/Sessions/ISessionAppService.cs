using System.Threading.Tasks;
using Abp.Application.Services;
using Nucleus.Sessions.Dto;

namespace Nucleus.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
