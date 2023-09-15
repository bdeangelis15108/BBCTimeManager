using Microsoft.Extensions.Configuration;

namespace Nucleus.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
