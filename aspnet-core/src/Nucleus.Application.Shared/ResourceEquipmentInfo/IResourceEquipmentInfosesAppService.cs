using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.ResourceEquipmentInfo.Dtos;
using Nucleus.Dto;


namespace Nucleus.ResourceEquipmentInfo
{
    public interface IResourceEquipmentInfosesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetResourceEquipmentInfosForViewDto>> GetAll(GetAllResourceEquipmentInfosesInput input);

        Task<GetResourceEquipmentInfosForViewDto> GetResourceEquipmentInfosForView(int id);

		Task<GetResourceEquipmentInfosForEditOutput> GetResourceEquipmentInfosForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditResourceEquipmentInfosDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetResourceEquipmentInfosesToExcel(GetAllResourceEquipmentInfosesForExcelInput input);

		
    }
}