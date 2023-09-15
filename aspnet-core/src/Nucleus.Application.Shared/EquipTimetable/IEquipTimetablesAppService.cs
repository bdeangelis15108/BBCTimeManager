using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.EquipTimetable.Dtos;
using Nucleus.Dto;

namespace Nucleus.EquipTimetable
{
    public interface IEquipTimetablesAppService : IApplicationService
    {
        Task<PagedResultDto<GetEquipTimetablesForViewDto>> GetAll(GetAllEquipTimetablesInput input);

        Task<GetEquipTimetablesForViewDto> GetEquipTimetablesForView(int id);

        Task<GetEquipTimetablesForEditOutput> GetEquipTimetablesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditEquipTimetablesDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetEquipTimetablesToExcel(GetAllEquipTimetablesForExcelInput input);

        Task<PagedResultDto<EquipTimetablesPayPeriodsLookupTableDto>> GetAllPayPeriodsForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<EquipTimetablesResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<EquipTimetablesJobPhaseCodesLookupTableDto>> GetAllJobPhaseCodesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<EquipTimetablesJobCategoriesLookupTableDto>> GetAllJobCategoriesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<EquipTimetablesJobsLookupTableDto>> GetAllJobsForLookupTable(GetAllForLookupTableInput input);

    }
}