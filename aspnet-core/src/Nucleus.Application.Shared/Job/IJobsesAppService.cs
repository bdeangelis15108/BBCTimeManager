using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.Job.Dtos;
using Nucleus.Dto;


namespace Nucleus.Job
{
    public interface IJobsesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetJobsForViewDto>> GetAll(GetAllJobsesInput input);

        Task<GetJobsForViewDto> GetJobsForView(int id);

		Task<GetJobsForEditOutput> GetJobsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditJobsDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetJobsesToExcel(GetAllJobsesForExcelInput input);

		
		Task<PagedResultDto<JobsAddressesLookupTableDto>> GetAllAddressesForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<JobsJobClassesLookupTableDto>> GetAllJobClassesForLookupTable(GetAllForLookupTableInput input);
		
    }
}