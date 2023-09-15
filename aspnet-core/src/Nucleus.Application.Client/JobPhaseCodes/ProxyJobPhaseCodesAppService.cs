using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.JobPhaseCode;
using Nucleus.JobPhaseCode.Dtos;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;

namespace Nucleus.JobPhaseCodes
{
    public class ProxyJobPhaseCodesAppService : ProxyAppServiceBase, IJobPhaseCodesAppService
    {
        public async Task CreateOrEdit(CreateOrEditJobPhaseCodesDto input)
        {
            await ApiClient.PostAsync<CreateOrEditJobPhaseCodesDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

        public async Task<PagedResultDto<GetJobPhaseCodesForViewDto>> GetAll(GetAllJobPhaseCodesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetJobPhaseCodesForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<PagedResultDto<JobPhaseCodesJobsLookupTableDto>> GetAllJobsForLookupTable(JobPhaseCode.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<JobPhaseCodesJobsLookupTableDto>>(GetEndpoint(nameof(GetAllJobsForLookupTable)), input);
        }

        public async Task<GetJobPhaseCodesForEditOutput> GetJobPhaseCodesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetJobPhaseCodesForEditOutput>(GetEndpoint(nameof(GetJobPhaseCodesForEdit)), input);
        }

        public async Task<GetJobPhaseCodesForViewDto> GetJobPhaseCodesForView(int id)
        {
            return await ApiClient.GetAsync<GetJobPhaseCodesForViewDto>(GetEndpoint(nameof(GetJobPhaseCodesForView)), id);
        }

        public async Task<FileDto> GetJobPhaseCodesToExcel(GetAllJobPhaseCodesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetJobPhaseCodesToExcel)), input);
        }
    }
}