using System.Threading.Tasks;
using Abp.Application.Services;
using Nucleus.Configuration.Tenants.Dto;

namespace Nucleus.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
