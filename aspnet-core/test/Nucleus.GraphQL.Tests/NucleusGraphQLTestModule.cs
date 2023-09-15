using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nucleus.Configure;
using Nucleus.Startup;
using Nucleus.Test.Base;

namespace Nucleus.GraphQL.Tests
{
    [DependsOn(
        typeof(NucleusGraphQLModule),
        typeof(NucleusTestBaseModule))]
    public class NucleusGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NucleusGraphQLTestModule).GetAssembly());
        }
    }
}