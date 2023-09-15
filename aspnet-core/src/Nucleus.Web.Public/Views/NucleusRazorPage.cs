using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Nucleus.Web.Public.Views
{
    public abstract class NucleusRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected NucleusRazorPage()
        {
            LocalizationSourceName = NucleusConsts.LocalizationSourceName;
        }
    }
}
