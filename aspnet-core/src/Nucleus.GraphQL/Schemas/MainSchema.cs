using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using Nucleus.Queries.Container;

namespace Nucleus.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}