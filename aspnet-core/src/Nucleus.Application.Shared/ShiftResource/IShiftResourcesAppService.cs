using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.ShiftResource.Dtos;
using Nucleus.Dto;


namespace Nucleus.ShiftResource
{
    public interface IShiftResourcesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetShiftResourcesForViewDto>> GetAll(GetAllShiftResourcesInput input);

        Task<GetShiftResourcesForViewDto> GetShiftResourcesForView(int id);

		Task<GetShiftResourcesForEditOutput> GetShiftResourcesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditShiftResourcesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetShiftResourcesToExcel(GetAllShiftResourcesForExcelInput input);

		
		Task<PagedResultDto<ShiftResourcesResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ShiftResourcesPayTypesLookupTableDto>> GetAllPayTypesForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ShiftResourcesJobPhaseCodesLookupTableDto>> GetAllJobPhaseCodesForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ShiftResourcesJobCategoriesLookupTableDto>> GetAllJobCategoriesForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ShiftResourcesTimesheetsLookupTableDto>> GetAllTimesheetsForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ShiftResourcesShiftsLookupTableDto>> GetAllShiftsForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ShiftResourcesWorkerClaseesLookupTableDto>> GetAllWorkerClaseesForLookupTable(GetAllForLookupTableInput input);
		
    }
}