using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.WorkerClasee.Dtos;

namespace Nucleus.WorkerClasee
{
    public class ProxyWorkerClaseesesAppService : ProxyAppServiceBase, IWorkerClaseesesAppService
    {


        public async Task CreateOrEdit(CreateOrEditWorkerClaseesDto input)
        {
            await ApiClient.PostAsync<CreateOrEditWorkerClaseesDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

      
        public async  Task<PagedResultDto<GetWorkerClaseesForViewDto>> GetAll(GetAllWorkerClaseesesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetWorkerClaseesForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }
       

        public async Task<GetWorkerClaseesForEditOutput> GetWorkerClaseesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetWorkerClaseesForEditOutput>(GetEndpoint(nameof(GetWorkerClaseesForEdit)), input);
        }
    }
}