using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Authorization.Users.Dto;
using Nucleus.Dto;
using Nucleus.Job;
using Nucleus.Job.Dtos;
using Nucleus.ResourceWorkerInfo.Dtos;

namespace Nucleus.Jobses
{
    public class ProxyJobsesAppService : ProxyAppServiceBase, IJobsesAppService
    {
        public async Task CreateOrEdit(CreateOrEditJobsDto input)
        {
             await ApiClient.PostAsync<CreateOrEditJobsDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

        public async Task<PagedResultDto<GetJobsForViewDto>> GetAll(GetAllJobsesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetJobsForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<PagedResultDto<JobsAddressesLookupTableDto>> GetAllAddressesForLookupTable(Job.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<JobsAddressesLookupTableDto>>(GetEndpoint(nameof(GetAllAddressesForLookupTable)), input);
        }

        public async Task<PagedResultDto<JobsJobClassesLookupTableDto>> GetAllJobClassesForLookupTable(Job.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<JobsJobClassesLookupTableDto>>(GetEndpoint(nameof(GetAllJobClassesForLookupTable)), input);
        }

        public async Task<FileDto> GetJobsesToExcel(GetAllJobsesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetJobsesToExcel)), input);
        }

        public async Task<GetJobsForEditOutput> GetJobsForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetJobsForEditOutput>(GetEndpoint(nameof(GetJobsForEdit)), input);
        }

        public async Task<GetJobsForViewDto> GetJobsForView(int id)
        {
            return await ApiClient.GetAsync<GetJobsForViewDto>(GetEndpoint(nameof(GetJobsForView)), id);
        }
    }
}