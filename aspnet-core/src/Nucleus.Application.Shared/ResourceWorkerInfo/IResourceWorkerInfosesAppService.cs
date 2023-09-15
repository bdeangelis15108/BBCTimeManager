using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.Dto;

namespace Nucleus.ResourceWorkerInfo
{
    public interface IResourceWorkerInfosesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetResourceWorkerInfosForViewDto>> GetAll(GetAllResourceWorkerInfosesInput input);

        Task<GetResourceWorkerInfosForViewDto> GetResourceWorkerInfosForView(int id);

		Task<GetResourceWorkerInfosForEditOutput> GetResourceWorkerInfosForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditResourceWorkerInfosDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetResourceWorkerInfosesToExcel(GetAllResourceWorkerInfosesForExcelInput input);

		
		Task<PagedResultDto<ResourceWorkerInfosWorkerClaseesLookupTableDto>> GetAllWorkerClaseesForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ResourceWorkerInfosResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input);
		
    }
}