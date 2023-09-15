using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.JobPhaseCode.Dtos;
using Nucleus.Dto;


namespace Nucleus.JobPhaseCode
{
    public interface IJobPhaseCodesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetJobPhaseCodesForViewDto>> GetAll(GetAllJobPhaseCodesInput input);

        Task<GetJobPhaseCodesForViewDto> GetJobPhaseCodesForView(int id);

		Task<GetJobPhaseCodesForEditOutput> GetJobPhaseCodesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditJobPhaseCodesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetJobPhaseCodesToExcel(GetAllJobPhaseCodesForExcelInput input);

		
		Task<PagedResultDto<JobPhaseCodesJobsLookupTableDto>> GetAllJobsForLookupTable(GetAllForLookupTableInput input);
		
    }
}