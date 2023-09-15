using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Nucleus
{
    public class NucleusCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NucleusCoreSharedModule).GetAssembly());
        }
    }
}