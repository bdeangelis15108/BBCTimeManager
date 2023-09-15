

using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.ResourceEquipmentInfo;
using Nucleus.ResourceEquipmentInfo.Dtos;
using System.Threading.Tasks;

namespace Nucleus.ResourceEquipmentsInfo
{
    public class ProxyResourceEquipmentInfosesAppService : ProxyAppServiceBase, IResourceEquipmentInfosesAppService
    {
        public async Task CreateOrEdit(CreateOrEditResourceEquipmentInfosDto input)
        {
            await ApiClient.PostAsync<CreateOrEditResourceEquipmentInfosDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

        public async Task<PagedResultDto<GetResourceEquipmentInfosForViewDto>> GetAll(GetAllResourceEquipmentInfosesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetResourceEquipmentInfosForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<FileDto> GetResourceEquipmentInfosesToExcel(GetAllResourceEquipmentInfosesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetResourceEquipmentInfosesToExcel)), input);
        }

        public async Task<GetResourceEquipmentInfosForEditOutput> GetResourceEquipmentInfosForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetResourceEquipmentInfosForEditOutput>(GetEndpoint(nameof(GetResourceEquipmentInfosForEdit)), input);
        }

        public async Task<GetResourceEquipmentInfosForViewDto> GetResourceEquipmentInfosForView(int id)
        {
            return await ApiClient.GetAsync<GetResourceEquipmentInfosForViewDto>(GetEndpoint(nameof(GetResourceEquipmentInfosForView)), id);
        }
    }
}