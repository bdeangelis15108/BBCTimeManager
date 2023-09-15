using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Authorization.Users.Dto;
using Nucleus.Dto;
using Nucleus.EmployeeUnion.Dtos;
using Nucleus.JobUnion;
using Nucleus.JobUnion.Dtos;
using Nucleus.ResourceWorkerInfo.Dtos;

namespace Nucleus.EmployeeUnion
{
    public class ProxyJobUnionsAppService : ProxyAppServiceBase, IJobUnionsAppService
    {
        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

        public async Task<PagedResultDto<GetJobUnionsForViewDto>> GetAll(GetAllJobUnionsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetJobUnionsForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<GetJobUnionsForViewDto> GetJobUnionsForView(int id)
        {
            return await ApiClient.GetAsync<GetJobUnionsForViewDto>(GetEndpoint(nameof(GetJobUnionsForView)), id);
        }

        public async Task<GetJobUnionsForEditOutput> GetJobUnionsForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetJobUnionsForEditOutput>(GetEndpoint(nameof(GetJobUnionsForEdit)), input);
        }

        public async Task CreateOrEdit(CreateOrEditJobUnionsDto input)
        {
            await ApiClient.PostAsync<CreateOrEditJobUnionsDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task<FileDto> GetJobUnionsToExcel(GetAllJobUnionsForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetJobUnionsToExcel)), input);
        }

        public async Task<PagedResultDto<JobUnionsJobsLookupTableDto>> GetAllJobsForLookupTable(JobUnion.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<JobUnionsJobsLookupTableDto>>(GetEndpoint(nameof(GetAllJobsForLookupTable)), input);
        }

        public async Task<PagedResultDto<JobUnionsUnionsLookupTableDto>> GetAllUnionsForLookupTable(JobUnion.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<JobUnionsUnionsLookupTableDto>>(GetEndpoint(nameof(GetAllUnionsForLookupTable)), input);
        }
    }
}