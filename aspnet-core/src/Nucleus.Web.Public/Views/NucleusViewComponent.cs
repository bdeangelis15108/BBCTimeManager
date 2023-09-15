using Abp.AspNetCore.Mvc.ViewComponents;

namespace Nucleus.Web.Public.Views
{
    public abstract class NucleusViewComponent : AbpViewComponent
    {
        protected NucleusViewComponent()
        {
            LocalizationSourceName = NucleusConsts.LocalizationSourceName;
        }
    }
}