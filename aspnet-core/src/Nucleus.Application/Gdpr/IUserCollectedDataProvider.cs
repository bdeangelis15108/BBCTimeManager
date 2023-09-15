using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Nucleus.Dto;

namespace Nucleus.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
