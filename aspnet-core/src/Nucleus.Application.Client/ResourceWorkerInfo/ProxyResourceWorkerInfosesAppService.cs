using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Authorization.Users.Dto;
using Nucleus.Dto;
using Nucleus.ResourceWorkerInfo.Dtos;

namespace Nucleus.ResourceWorkerInfo
{
    public class ProxyResourceWorkerInfosesAppService : ProxyAppServiceBase, IResourceWorkerInfosesAppService
    {
       

        public async Task<PagedResultDto<GetResourceWorkerInfosForViewDto>> GetAll(GetAllResourceWorkerInfosesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetResourceWorkerInfosForViewDto>>(GetEndpoint(nameof(GetAll)), input); 
        }

        public async Task<GetResourceWorkerInfosForViewDto> GetResourceWorkerInfosForView(int id)
        {
            return await ApiClient.GetAsync<GetResourceWorkerInfosForViewDto>(GetEndpoint(nameof(GetResourceWorkerInfosForView)), id);
        }

        public async Task<GetResourceWorkerInfosForEditOutput> GetResourceWorkerInfosForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetResourceWorkerInfosForEditOutput>(GetEndpoint(nameof(GetResourceWorkerInfosForEdit)), input);
        }

        public async Task CreateOrEdit(CreateOrEditResourceWorkerInfosDto input)
        {
            await ApiClient.PostAsync<CreateOrEditResourceWorkerInfosDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

        public async Task<FileDto> GetResourceWorkerInfosesToExcel(GetAllResourceWorkerInfosesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetResourceWorkerInfosesToExcel)), input);
        }

        public async Task<PagedResultDto<ResourceWorkerInfosWorkerClaseesLookupTableDto>> GetAllWorkerClaseesForLookupTable(GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync < PagedResultDto < ResourceWorkerInfosWorkerClaseesLookupTableDto>> (GetEndpoint(nameof(GetAllWorkerClaseesForLookupTable)), input);
        }

        public async Task<PagedResultDto<ResourceWorkerInfosResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ResourceWorkerInfosResourcesLookupTableDto>>(GetEndpoint(nameof(GetAllResourcesForLookupTable)), input);
        }
    }
}