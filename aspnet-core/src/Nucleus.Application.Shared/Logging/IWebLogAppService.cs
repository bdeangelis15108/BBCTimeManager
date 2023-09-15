using Abp.Application.Services;
using Nucleus.Dto;
using Nucleus.Logging.Dto;

namespace Nucleus.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
