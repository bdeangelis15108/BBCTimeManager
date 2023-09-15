using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;

namespace Nucleus.PayTypes
{
    public class ProxyPayTypesesAppService : ProxyAppServiceBase, IPayTypesesAppService
    {
        public async Task CreateOrEdit(CreateOrEditPayTypesDto input)
        {
            await ApiClient.PostAsync<CreateOrEditPayTypesDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

        public async Task<PagedResultDto<GetPayTypesForViewDto>> GetAll(GetAllPayTypesesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetPayTypesForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<FileDto> GetPayTypesesToExcel(GetAllPayTypesesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetPayTypesesToExcel)), input);
        }

        public async Task<GetPayTypesForEditOutput> GetPayTypesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetPayTypesForEditOutput>(GetEndpoint(nameof(GetPayTypesForEdit)), input);
        }

        public async Task<GetPayTypesForViewDto> GetPayTypesForView(int id)
        {
            return await ApiClient.GetAsync<GetPayTypesForViewDto>(GetEndpoint(nameof(GetPayTypesForView)), id);
        }
    }
}