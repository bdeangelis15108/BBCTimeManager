using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.Timetable.Dtos;
using Nucleus.Dto;

namespace Nucleus.Timetable
{
    public interface ITimetablesAppService : IApplicationService
    {
        Task<PagedResultDto<dynamic>> GetAllForJob(TimetableFilterRequestDto request);
        Task<PagedResultDto<GetTimetablesForViewDto>> GetAll(GetAllTimetablesInput input);

        Task<GetTimetablesForViewDto> GetTimetablesForView(int id);

        Task<GetTimetablesForEditOutput> GetTimetablesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTimetablesDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTimetablesToExcel(GetAllTimetablesForExcelInput input);

        Task<PagedResultDto<TimetablesPayPeriodsLookupTableDto>> GetAllPayPeriodsForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TimetablesResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input);
        Task<UnionPayRate.Dtos.UnionPayRatesDto> GetUnionPayRate(int unionsId, int classId);

        Task<PagedResultDto<TimetablesUnionPayRatesLookupTableDto>> GetAllUnionPayRatesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TimetablesUnionsLookupTableDto>> GetAllUnionsForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TimetablesAddressesLookupTableDto>> GetAllAddressesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TimetablesExpenseTypesLookupTableDto>> GetAllExpenseTypesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TimetablesCostTypesLookupTableDto>> GetAllCostTypesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TimetablesAccountsLookupTableDto>> GetAllAccountsForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TimetablesUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TimetablesPayTypesLookupTableDto>> GetAllPayTypesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TimetablesWorkerClaseesLookupTableDto>> GetAllWorkerClaseesForLookupTable(GetAllForLookupTableInput input);

    }
}