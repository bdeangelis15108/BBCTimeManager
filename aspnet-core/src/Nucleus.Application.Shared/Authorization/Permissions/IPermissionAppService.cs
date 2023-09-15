using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.Authorization.Permissions.Dto;

namespace Nucleus.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
