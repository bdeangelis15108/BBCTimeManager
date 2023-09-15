using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.ExpenseType;
using Nucleus.ExpenseType.Dtos;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.Resource;
using Nucleus.Resource.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;

namespace Nucleus.Resourceses
{
   public  class ProxyResourcesesAppService : ProxyAppServiceBase, IResourcesesAppService
    {
       public async Task CreateOrEdit(CreateOrEditResourcesDto input)
        {
            await ApiClient.PostAsync<CreateOrEditResourcesDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

       public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

       public async Task<PagedResultDto<GetResourcesForViewDto>> GetAll(GetAllResourcesesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetResourcesForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

       public async Task<FileDto> GetResourcesesToExcel(GetAllResourcesesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetResourcesesToExcel)), input);
        }

       public async Task<GetResourcesForEditOutput> GetResourcesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetResourcesForEditOutput> (GetEndpoint(nameof(GetResourcesForEdit)), input);
        }

       public async Task<GetResourcesForViewDto> GetResourcesForView(int id)
        {
            return await ApiClient.GetAsync<GetResourcesForViewDto>(GetEndpoint(nameof(GetResourcesForView)), id);
        }
    }
}