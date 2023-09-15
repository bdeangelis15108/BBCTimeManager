using Abp.AspNetCore.Mvc.Views;

namespace Nucleus.Web.Views
{
    public abstract class NucleusRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected NucleusRazorPage()
        {
            LocalizationSourceName = NucleusConsts.LocalizationSourceName;
        }
    }
}
