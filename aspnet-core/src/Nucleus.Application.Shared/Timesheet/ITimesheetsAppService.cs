using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.Timesheet.Dtos;
using Nucleus.Dto;


namespace Nucleus.Timesheet
{
    public interface ITimesheetsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTimesheetsForViewDto>> GetAll(GetAllTimesheetsInput input);

        Task<GetTimesheetsForViewDto> GetTimesheetsForView(int id);

		Task<GetTimesheetsForEditOutput> GetTimesheetsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTimesheetsDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTimesheetsToExcel(GetAllTimesheetsForExcelInput input);

		
		Task<PagedResultDto<TimesheetsStatusesLookupTableDto>> GetAllStatusesForLookupTable(GetAllForLookupTableInput input);
		
    }
}