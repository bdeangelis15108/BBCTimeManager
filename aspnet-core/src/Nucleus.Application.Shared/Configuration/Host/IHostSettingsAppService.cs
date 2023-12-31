﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Nucleus.Configuration.Host.Dto;

namespace Nucleus.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
