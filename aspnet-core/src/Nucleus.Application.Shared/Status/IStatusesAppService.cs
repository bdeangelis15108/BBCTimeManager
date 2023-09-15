using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.Status.Dtos;
using Nucleus.Dto;


namespace Nucleus.Status
{
    public interface IStatusesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetStatusesForViewDto>> GetAll(GetAllStatusesInput input);

        Task<GetStatusesForViewDto> GetStatusesForView(int id);

		Task<GetStatusesForEditOutput> GetStatusesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditStatusesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetStatusesToExcel(GetAllStatusesForExcelInput input);

		
    }
}