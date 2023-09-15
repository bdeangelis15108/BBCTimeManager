using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.Union.Dtos;
using Nucleus.Dto;


namespace Nucleus.Union
{
    public interface IUnionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUnionsForViewDto>> GetAll(GetAllUnionsInput input);

        Task<GetUnionsForViewDto> GetUnionsForView(int id);

		Task<GetUnionsForEditOutput> GetUnionsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditUnionsDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetUnionsToExcel(GetAllUnionsForExcelInput input);

		
    }
}