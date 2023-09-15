using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Nucleus
{
    [DependsOn(typeof(NucleusXamarinSharedModule))]
    public class NucleusXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NucleusXamarinAndroidModule).GetAssembly());
        }
    }
}