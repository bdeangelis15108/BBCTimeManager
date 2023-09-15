using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Nucleus
{
    [DependsOn(typeof(NucleusXamarinSharedModule))]
    public class NucleusXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NucleusXamarinIosModule).GetAssembly());
        }
    }
}