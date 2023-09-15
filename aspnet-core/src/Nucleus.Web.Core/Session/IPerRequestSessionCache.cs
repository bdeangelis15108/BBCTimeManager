using System.Threading.Tasks;
using Nucleus.Sessions.Dto;

namespace Nucleus.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
