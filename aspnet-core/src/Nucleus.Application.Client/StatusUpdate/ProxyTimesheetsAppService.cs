using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ShiftResource.Dtos;
using Nucleus.StatusUpdate;
using Nucleus.StatusUpdate.Dtos;
using Nucleus.Timesheet;
using Nucleus.Timesheet.Dtos;

namespace Nucleus.StatusUpdate
{
    public class ProxyStatusUpdatesAppService : ProxyAppServiceBase, IStatusUpdatesAppService
    {

        public async Task CreateOrEdit(CreateOrEditTimesheetsDto input)
        {
            await ApiClient.PostAsync<CreateOrEditTimesheetsDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task CreateOrEdit(CreateOrEditStatusUpdatesDto input)
        {
            await ApiClient.PostAsync<CreateOrEditStatusUpdatesDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.DeleteAsync<EntityDto>(GetEndpoint(nameof(Delete)), new { Id = input.Id });
        }

      
        public async Task<PagedResultDto<GetStatusUpdatesForViewDto>> GetAll(GetAllStatusUpdatesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetStatusUpdatesForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<PagedResultDto<StatusUpdatesJobsLookupTableDto>> GetAllJobsForLookupTable(StatusUpdate.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<StatusUpdatesJobsLookupTableDto>>(GetEndpoint(nameof(GetAllJobsForLookupTable)), input);
        }

        public async Task<PagedResultDto<StatusUpdatesStatusesLookupTableDto>> GetAllStatusesForLookupTable(StatusUpdate.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<StatusUpdatesStatusesLookupTableDto>>(GetEndpoint(nameof(GetAllStatusesForLookupTable)), input);
        }

        public async Task<PagedResultDto<StatusUpdatesTimesheetsLookupTableDto>> GetAllTimesheetsForLookupTable(StatusUpdate.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<StatusUpdatesTimesheetsLookupTableDto>>(GetEndpoint(nameof(GetAllTimesheetsForLookupTable)), input);
        }

        public async Task<PagedResultDto<StatusUpdatesUserLookupTableDto>> GetAllUserForLookupTable(StatusUpdate.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<StatusUpdatesUserLookupTableDto>>(GetEndpoint(nameof(GetAllUserForLookupTable)), input);
        }

        public async Task<GetStatusUpdatesForEditOutput> GetStatusUpdatesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetStatusUpdatesForEditOutput>(GetEndpoint(nameof(GetStatusUpdatesForEdit)), input);
        }

        public async Task<GetStatusUpdatesForViewDto> GetStatusUpdatesForView(int id)
        {
            return await ApiClient.GetAsync<GetStatusUpdatesForViewDto>(GetEndpoint(nameof(GetStatusUpdatesForView)), id);
        }

        public async Task<FileDto> GetStatusUpdatesToExcel(GetAllStatusUpdatesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto> (GetEndpoint(nameof(GetStatusUpdatesToExcel)), input);
        }

        public async Task<dynamic> LoadNewPayPeriod()
        {
            return await ApiClient.GetAsync<dynamic>(GetEndpoint(nameof(LoadNewPayPeriod)));
        }
        public async Task<dynamic> CeDataRefresh()
        {
            return await ApiClient.GetAsync<dynamic>(GetEndpoint(nameof(CeDataRefresh)));
        }
    }
}