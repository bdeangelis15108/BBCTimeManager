using Abp.Auditing;
using Nucleus.Configuration.Dto;

namespace Nucleus.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}