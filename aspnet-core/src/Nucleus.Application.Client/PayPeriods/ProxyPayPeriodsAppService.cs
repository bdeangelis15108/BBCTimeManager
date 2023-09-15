using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.PayPeriod;
using Nucleus.PayPeriod.Dtos;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;

namespace Nucleus.PayPeriods
{
    public class ProxyPayPeriodsAppService : ProxyAppServiceBase, IPayPeriodsAppService
    {
        public async Task CreateOrEdit(CreateOrEditPayPeriodsDto input)
        {
            await ApiClient.PostAsync<CreateOrEditPayPeriodsDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

        public async Task<PagedResultDto<GetPayPeriodsForViewDto>> GetAll(GetAllPayPeriodsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetPayPeriodsForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<GetPayPeriodsForEditOutput> GetPayPeriodsForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetPayPeriodsForEditOutput>(GetEndpoint(nameof(GetPayPeriodsForEdit)), input);
        }

        public async Task<GetPayPeriodsForViewDto> GetPayPeriodsForView(int id)
        {
            return await ApiClient.GetAsync<GetPayPeriodsForViewDto>(GetEndpoint(nameof(GetPayPeriodsForView)), id);
        }

        public async Task<FileDto> GetPayPeriodsToExcel(GetAllPayPeriodsForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetPayPeriodsToExcel)), input);
        }
        }
    }