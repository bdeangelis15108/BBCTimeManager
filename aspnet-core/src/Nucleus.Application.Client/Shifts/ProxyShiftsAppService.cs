using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.Shift;
using Nucleus.Shift.Dtos;
using Nucleus.ShiftResource.Dtos;

namespace Nucleus.Shifts
{
    public class ProxyShiftsAppService : ProxyAppServiceBase, IShiftsAppService
    {
        
        public async Task CreateOrEdit(CreateOrEditShiftsDto input)
        {
            await ApiClient.PostAsync<CreateOrEditShiftsDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }


        public async Task<PagedResultDto<GetShiftsForViewDto>> GetAll(GetAllShiftsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetShiftsForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<PagedResultDto<ShiftsJobsLookupTableDto>> GetAllJobsForLookupTable(Shift.Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ShiftsJobsLookupTableDto>>(GetEndpoint(nameof(GetAllJobsForLookupTable)), input);
        }

        public async Task<GetShiftsForEditOutput> GetShiftsForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetShiftsForEditOutput>(GetEndpoint(nameof(GetShiftsForEdit)), input);
        }

        public async Task<GetShiftsForViewDto> GetShiftsForView(int id)
        {
            return await ApiClient.GetAsync<GetShiftsForViewDto>(GetEndpoint(nameof(GetShiftsForView)),id);
        }

        public async Task<FileDto> GetShiftsToExcel(GetAllShiftsForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetShiftsToExcel)), input);
        }
    }
}