using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Authorization.Users.Dto;
using Nucleus.Dto;
using Nucleus.EmployeeUnion.Dtos;
using Nucleus.ResourceWorkerInfo.Dtos;

namespace Nucleus.EmployeeUnion
{
    public class ProxyEmployeeUnionsAppService : ProxyAppServiceBase, IEmployeeUnionsAppService
    {
        
        public async Task<PagedResultDto<GetEmployeeUnionsForViewDto>> GetAll(GetAllEmployeeUnionsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetEmployeeUnionsForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<GetEmployeeUnionsForViewDto> GetEmployeeUnionsForView(int id)
        {
            return await ApiClient.GetAsync<GetEmployeeUnionsForViewDto>(GetEndpoint(nameof(GetEmployeeUnionsForView)), id);
        }

        public async Task<GetEmployeeUnionsForEditOutput> GetEmployeeUnionsForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetEmployeeUnionsForEditOutput>(GetEndpoint(nameof(GetEmployeeUnionsForEdit)), input);
        }

        public async Task CreateOrEdit(CreateOrEditEmployeeUnionsDto input)
        {
            await ApiClient.PostAsync<CreateOrEditEmployeeUnionsDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task<FileDto> GetEmployeeUnionsToExcel(GetAllEmployeeUnionsForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetEmployeeUnionsToExcel)), input);
        }

        public async Task<PagedResultDto<EmployeeUnionsUnionsLookupTableDto>> GetAllUnionsForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<EmployeeUnionsUnionsLookupTableDto>> (GetEndpoint(nameof(GetAllUnionsForLookupTable)), input);
        }

        public async Task<PagedResultDto<EmployeeUnionsResourcesLookupTableDto>> GetAllResourcesForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<EmployeeUnionsResourcesLookupTableDto>>(GetEndpoint(nameof(GetAllResourcesForLookupTable)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }
    }
}