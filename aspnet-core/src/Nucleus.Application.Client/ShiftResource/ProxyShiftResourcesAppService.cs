using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ShiftResource.Dtos;

namespace Nucleus.ShiftResource
{
    public class ProxyShiftResourcesAppService : ProxyAppServiceBase, IShiftResourcesAppService
    {
        public async Task CreateOrEdit(CreateOrEditShiftResourcesDto input)
        {
            await ApiClient.PostAsync<CreateOrEditShiftResourcesDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.DeleteAsync<EntityDto>(GetEndpoint(nameof(Delete)), new { Id = input.Id });
        }      
        public async Task<PagedResultDto<GetShiftResourcesForViewDto>> GetAll(GetAllShiftResourcesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetShiftResourcesForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<PagedResultDto<ShiftResourcesJobCategoriesLookupTableDto>> GetAllJobCategoriesForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ShiftResourcesJobCategoriesLookupTableDto>>(GetEndpoint(nameof(GetAllJobCategoriesForLookupTable)), input);
        }

        public async Task<PagedResultDto<ShiftResourcesJobPhaseCodesLookupTableDto>> GetAllJobPhaseCodesForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ShiftResourcesJobPhaseCodesLookupTableDto>>(GetEndpoint(nameof(GetAllJobPhaseCodesForLookupTable)), input);
        }

        public async Task<PagedResultDto<ShiftResourcesPayTypesLookupTableDto>> GetAllPayTypesForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ShiftResourcesPayTypesLookupTableDto>>(GetEndpoint(nameof(GetAllPayTypesForLookupTable)), input);
        }

        public async Task<PagedResultDto<ShiftResourcesResourcesLookupTableDto>> GetAllResourcesForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ShiftResourcesResourcesLookupTableDto>>(GetEndpoint(nameof(GetAllResourcesForLookupTable)), input);
        }

        public async Task<PagedResultDto<ShiftResourcesShiftsLookupTableDto>> GetAllShiftsForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ShiftResourcesShiftsLookupTableDto>>(GetEndpoint(nameof(GetAllShiftsForLookupTable)), input);
        }

        public async Task<PagedResultDto<ShiftResourcesTimesheetsLookupTableDto>> GetAllTimesheetsForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ShiftResourcesTimesheetsLookupTableDto>>(GetEndpoint(nameof(GetAllTimesheetsForLookupTable)), input);
        }

        public async Task<PagedResultDto<ShiftResourcesWorkerClaseesLookupTableDto>> GetAllWorkerClaseesForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ShiftResourcesWorkerClaseesLookupTableDto>>(GetEndpoint(nameof(GetAllWorkerClaseesForLookupTable)), input);
        }

        public async Task<GetShiftResourcesForEditOutput> GetShiftResourcesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetShiftResourcesForEditOutput>(GetEndpoint(nameof(GetShiftResourcesForEdit)), input);
        }

        public async Task<GetShiftResourcesForViewDto> GetShiftResourcesForView(int id)
        {
            return await ApiClient.GetAsync<GetShiftResourcesForViewDto>(GetEndpoint(nameof(GetShiftResourcesForView)), id);
        }

        public async Task<FileDto> GetShiftResourcesToExcel(GetAllShiftResourcesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetShiftResourcesToExcel)), input);
        }
    }
}