using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.Account.Dtos;
using Nucleus.Dto;

namespace Nucleus.Account
{
    public interface IAccountsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAccountsForViewDto>> GetAll(GetAllAccountsInput input);

        Task<GetAccountsForViewDto> GetAccountsForView(int id);

        Task<GetAccountsForEditOutput> GetAccountsForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAccountsDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAccountsToExcel(GetAllAccountsForExcelInput input);

    }
}