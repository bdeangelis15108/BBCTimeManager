using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ShiftResource.Dtos;
using Nucleus.Timesheet;
using Nucleus.Timesheet.Dtos;

namespace Nucleus.Timesheets
{
    public class ProxyTimesheetsAppService : ProxyAppServiceBase, ITimesheetsAppService
    {

        public async Task CreateOrEdit(CreateOrEditTimesheetsDto input)
        {
            await ApiClient.PostAsync<CreateOrEditTimesheetsDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

      
        public async Task<PagedResultDto<GetTimesheetsForViewDto>> GetAll(GetAllTimesheetsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetTimesheetsForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<PagedResultDto<TimesheetsStatusesLookupTableDto>> GetAllStatusesForLookupTable(Timesheet.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<TimesheetsStatusesLookupTableDto>>(GetEndpoint(nameof(GetAllStatusesForLookupTable)), input);
        }

        public async Task<GetTimesheetsForEditOutput> GetTimesheetsForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetTimesheetsForEditOutput>(GetEndpoint(nameof(GetTimesheetsForEdit)), input);
        }

        public async Task<GetTimesheetsForViewDto> GetTimesheetsForView(int id)
        {
            return await ApiClient.GetAsync<GetTimesheetsForViewDto> (GetEndpoint(nameof(GetTimesheetsForView)), id);
        }

        public async Task<FileDto> GetTimesheetsToExcel(GetAllTimesheetsForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetTimesheetsToExcel)), input);
        }
    }
}