using Abp.Domain.Services;

namespace Nucleus
{
    public abstract class NucleusDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected NucleusDomainServiceBase()
        {
            LocalizationSourceName = NucleusConsts.LocalizationSourceName;
        }
    }
}
