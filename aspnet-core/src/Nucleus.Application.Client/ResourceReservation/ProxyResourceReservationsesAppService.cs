using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;

namespace Nucleus.ResourceWorkerInfo
{
    public class ProxyResourceReservationsesAppService : ProxyAppServiceBase, IResourceReservationsesAppService
    {
        public async Task CreateOrEdit(CreateOrEditResourceReservationsDto input)
        {
            await ApiClient.PostAsync<CreateOrEditResourceReservationsDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.DeleteAsync<EntityDto>(GetEndpoint(nameof(Delete)), new { Id = input.Id });
        }

        public async Task<PagedResultDto<GetResourceReservationsForViewDto>> GetAll(GetAllResourceReservationsesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetResourceReservationsForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<PagedResultDto<ResourceReservationsResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ResourceReservationsResourcesLookupTableDto>>(GetEndpoint(nameof(GetAllResourcesForLookupTable)), input);
        }

        public async Task<PagedResultDto<ResourceReservationsUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ResourceReservationsUserLookupTableDto>>(GetEndpoint(nameof(GetAllUserForLookupTable)), input);
        }

        public async  Task<FileDto> GetResourceReservationsesToExcel(GetAllResourceReservationsesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetResourceReservationsesToExcel)), input);
        }

        public async Task<GetResourceReservationsForEditOutput> GetResourceReservationsForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetResourceReservationsForEditOutput>(GetEndpoint(nameof(GetResourceReservationsForEdit)), input);
        }

        public async Task<GetResourceReservationsForViewDto> GetResourceReservationsForView(int id)
        {
            return await ApiClient.GetAsync<GetResourceReservationsForViewDto>(GetEndpoint(nameof(GetResourceReservationsForView)), id);
        }
        public async Task<PagedResultDto<GetMyResourceReservationsDto>> GetMyReservedResource(GetAllResourceReservationsesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetMyResourceReservationsDto>>(GetEndpoint(nameof(GetMyReservedResource)), input);
        }

        public async Task DeleteByResourceId(int id)
        {
            await ApiClient.PostAsync<int>(GetEndpoint(nameof(DeleteByResourceId)), id);
        }
    }
}