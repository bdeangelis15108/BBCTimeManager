using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.StatusUpdate.Dtos;
using Nucleus.Dto;


namespace Nucleus.StatusUpdate
{
    public interface IStatusUpdatesAppService : IApplicationService 
    {
		Task<dynamic> LoadNewPayPeriod();
		Task<dynamic> CeDataRefresh();

		Task<PagedResultDto<GetStatusUpdatesForViewDto>> GetAll(GetAllStatusUpdatesInput input);

        Task<GetStatusUpdatesForViewDto> GetStatusUpdatesForView(int id);

		Task<GetStatusUpdatesForEditOutput> GetStatusUpdatesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditStatusUpdatesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetStatusUpdatesToExcel(GetAllStatusUpdatesForExcelInput input);

		
		Task<PagedResultDto<StatusUpdatesTimesheetsLookupTableDto>> GetAllTimesheetsForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<StatusUpdatesStatusesLookupTableDto>> GetAllStatusesForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<StatusUpdatesJobsLookupTableDto>> GetAllJobsForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<StatusUpdatesUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}