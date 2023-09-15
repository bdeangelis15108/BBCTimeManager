using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.PayPeriod;
using Nucleus.PayPeriod.Dtos;
using Nucleus.PayperiodHistory.Dtos;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;

namespace Nucleus.PayperiodHistory
{
    public class ProxyPayperiodHistoriesAppService : ProxyAppServiceBase, IPayperiodHistoriesAppService
    {
        public async Task CreateOrEdit(CreateOrEditPayperiodHistoriesDto input)
        {
            await ApiClient.PostAsync<CreateOrEditPayperiodHistoriesDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }


        public async Task<PagedResultDto<GetPayperiodHistoriesForViewDto>> GetAll(GetAllPayperiodHistoriesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetPayperiodHistoriesForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<PagedResultDto<PayperiodHistoriesPayPeriodsLookupTableDto>> GetAllPayPeriodsForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<PayperiodHistoriesPayPeriodsLookupTableDto>>(GetEndpoint(nameof(GetAllPayPeriodsForLookupTable)), input);
        }

        public async Task<GetPayperiodHistoriesForEditOutput> GetPayperiodHistoriesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetPayperiodHistoriesForEditOutput>(GetEndpoint(nameof(GetPayperiodHistoriesForEdit)), input);
        }

        public async Task<GetPayperiodHistoriesForViewDto> GetPayperiodHistoriesForView(int id)
        {
             return await ApiClient.GetAsync<GetPayperiodHistoriesForViewDto>(GetEndpoint(nameof(GetPayperiodHistoriesForView)), id);
        }

        public async Task<FileDto> GetPayperiodHistoriesToExcel(GetAllPayperiodHistoriesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetPayperiodHistoriesToExcel)), input);
        }

       
    }
}