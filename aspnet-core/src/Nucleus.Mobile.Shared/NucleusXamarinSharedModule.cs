using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Nucleus
{
    [DependsOn(typeof(NucleusClientModule), typeof(AbpAutoMapperModule))]
    public class NucleusXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NucleusXamarinSharedModule).GetAssembly());
        }
    }
}