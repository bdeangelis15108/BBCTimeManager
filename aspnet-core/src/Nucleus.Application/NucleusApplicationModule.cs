using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Nucleus.Authorization;

namespace Nucleus
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(NucleusApplicationSharedModule),
        typeof(NucleusCoreModule)
        )]
    public class NucleusApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NucleusApplicationModule).GetAssembly());
        }
    }
}