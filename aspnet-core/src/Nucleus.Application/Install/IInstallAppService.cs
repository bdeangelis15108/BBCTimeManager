using System.Threading.Tasks;
using Abp.Application.Services;
using Nucleus.Install.Dto;

namespace Nucleus.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}