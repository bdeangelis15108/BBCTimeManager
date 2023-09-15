using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Nucleus.Startup
{
    [DependsOn(typeof(NucleusCoreModule))]
    public class NucleusGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NucleusGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}