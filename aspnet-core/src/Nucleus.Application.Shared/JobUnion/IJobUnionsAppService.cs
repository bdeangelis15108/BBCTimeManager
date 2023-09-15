using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.JobUnion.Dtos;
using Nucleus.Dto;


namespace Nucleus.JobUnion
{
    public interface IJobUnionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetJobUnionsForViewDto>> GetAll(GetAllJobUnionsInput input);

        Task<GetJobUnionsForViewDto> GetJobUnionsForView(int id);

		Task<GetJobUnionsForEditOutput> GetJobUnionsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditJobUnionsDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetJobUnionsToExcel(GetAllJobUnionsForExcelInput input);

		
		Task<PagedResultDto<JobUnionsJobsLookupTableDto>> GetAllJobsForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<JobUnionsUnionsLookupTableDto>> GetAllUnionsForLookupTable(GetAllForLookupTableInput input);
		
    }
}