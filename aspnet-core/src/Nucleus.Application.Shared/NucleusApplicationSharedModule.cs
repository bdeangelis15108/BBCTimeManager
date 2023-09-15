using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Nucleus
{
    [DependsOn(typeof(NucleusCoreSharedModule))]
    public class NucleusApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NucleusApplicationSharedModule).GetAssembly());
        }
    }
}