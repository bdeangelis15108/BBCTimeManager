using System.Threading.Tasks;
using Abp.Application.Services;
using Nucleus.Editions.Dto;
using Nucleus.MultiTenancy.Dto;

namespace Nucleus.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}